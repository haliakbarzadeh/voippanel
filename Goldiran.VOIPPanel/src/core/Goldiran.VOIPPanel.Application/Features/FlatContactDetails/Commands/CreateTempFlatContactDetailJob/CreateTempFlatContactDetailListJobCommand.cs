
using AutoMapper;
using Goldiran.VOIPPanel.Application.Features.Positions.Commands.CreatePosition;
using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails;
using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails.Enums;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.TempFlatDataJobs.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Goldiran.VOIPPanel.ReadModel.IQueries;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Format;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Voip.Framework.Common.Extensions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using ContactDetail = Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails.ContactDetail;
using Goldiran.VOIPPanel.Application.Features.FlatContactDetails.Models;
using Goldiran.VOIPPanel.Application.Features.FlatContactDetails.Module;

namespace Goldiran.VOIPPanel.Application.Features.TempFlatContactDetails.Commands.CreateTempFlatContactDetail;

public class CreateTempFlatContactDetailListJobCommand : BaseCreateCommandRequest, IRequest<FlatContactDetailResponse>
{
    public bool IsRestricted { get; set; } = true;
    public ContactReportType ContactReportType { get; set; } = ContactReportType.Detail;
    public bool IsJob { get; set; } = false;
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public TimeSpan? FromTime { get; set; }
    public TimeSpan? ToTime { get; set; }
    public string? TypeCalls { get; set; }
    public string? Agents { get; set; }
    public string? Phone { get; set; }
    public int? OrderBy { get; set; }
    public OperationReportType? OperationReportType { get; set; }
    public int PageSize { get; set; } = 10;
    public class Handler : IRequestHandler<CreateTempFlatContactDetailListJobCommand, FlatContactDetailResponse>
    {
        private readonly IMapper _mapper;
        private readonly IContactDetailRepository _contactDetailRepository;
        private readonly IContactDetailQueryService _contactDetailQueryService;
        private readonly IReadModelContext _readModelContext;
        private readonly ITempFlatDataJobQueryService _TempFlatDataJobQueryService;
        private readonly ITempFlatDataJobRepository _TempFlatDataJobRepository;


        public Handler(IMapper mapper, IContactDetailRepository contactDetailRepository, IContactDetailQueryService contactDetailQueryService, ITempFlatDataJobQueryService TempFlatDataJobQueryService, ITempFlatDataJobRepository TempFlatDataJobRepository, IReadModelContext readModelContext)
        {
            _mapper = mapper;
            _contactDetailRepository = contactDetailRepository;
            _contactDetailQueryService = contactDetailQueryService;
            _readModelContext = readModelContext;
            _TempFlatDataJobQueryService = TempFlatDataJobQueryService;
            _TempFlatDataJobRepository = TempFlatDataJobRepository;
        }

        public async Task<FlatContactDetailResponse> Handle(CreateTempFlatContactDetailListJobCommand request, CancellationToken cancellationToken)
        {
            var list = await GetContactDetails(request, cancellationToken);
            if (list.Items.Count==0) {
                var notResult= new FlatContactDetailResponse() { Date = new DateTime(2000, 1, 1), Count = 0 };
                //await UpdateLastDataJob(request, notResult, cancellationToken);
                return notResult;
            }

            var date=request.FromDate.Value.Add(request.FromTime.Value);    
            var existList=_mapper.Map<List<ContactDetailDto>>(_readModelContext.Set< Goldiran.VOIPPanel.ReadModel.Entities.ContactDetail>().AsNoTracking().Where(c=>c.Date> date && c.ReportType==ReportType.Normal).OrderBy(c=>c.Id).Take(100).ToList());

            List<ContactDetailDto> addedList= new List<ContactDetailDto>();
            if (request.ContactReportType==(ContactReportType)((int)ReportType.Normal))
                addedList = list.Items.Except(existList, new FlatComparer()).ToList();
            else if (request.ContactReportType == (ContactReportType)((int)ReportType.AutoDial))
                addedList = list.Items.Except(existList, new FlatAutoDialComparer()).ToList();
            else
                addedList = list.Items.ToList();

            addedList=addedList.Where(c=>c.Dest.Length<50).ToList();

            //if (addedList.IsNullOrEmpty()) { return new FlatContactDetailResponse() { Date = new DateTime(2000, 1, 1), Count = 0 }; }
            //var command=list.Items.Select(c=>new ContactDetail(c.LinkedId, c.DstChannel, c.Status, c.Disposition, c.Date, c.Dcontext, c.Source, c.Dest, c.Billsecond, c.Duration, c.Waiting, c.Filepath, c.Recordingfile,(ReportType)((int)request.ContactReportType))).OrderBy(c=>c.Date).ToList();
            var command = addedList.Select(c => new ContactDetail(c.LinkedId, c.DstChannel, c.Status, c.Disposition, c.Date, c.Dcontext, c.Source, c.Dest, c.Billsecond, c.Duration, c.Waiting, c.Filepath, c.Recordingfile, (ReportType)((int)request.ContactReportType), c.QueueName, string.Empty, 0, 0, 0, 0)).OrderBy(c => c.Date).ToList();
            var tempCommand = list.Items.Select(c => new ContactDetail(c.LinkedId, c.DstChannel, c.Status, c.Disposition, c.Date, c.Dcontext, c.Source, c.Dest, c.Billsecond, c.Duration, c.Waiting, c.Filepath, c.Recordingfile, (ReportType)((int)request.ContactReportType),c.QueueName,string.Empty, 0, 0, 0, 0)).OrderBy(c => c.Date).ToList();

            var result = new FlatContactDetailResponse() { Date = tempCommand.LastOrDefault().Date, Count = list.Items.Count };

            if (!command.IsNullOrEmpty())
            {
                //await UpdateLastDataJob(request, result, cancellationToken);

                _contactDetailRepository.AddRange(command);
                await _contactDetailRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }



            return result;
        }

        private async Task<PaginatedList<ContactDetailDto>> GetContactDetails(CreateTempFlatContactDetailListJobCommand request, CancellationToken cancellationToken)
        {
            var query = new GetContactDetailsQuery()
            {
                IsRestricted = request.IsRestricted,
                IsJob = request.IsJob,
                ContactReportType = request.ContactReportType,
                FromDate = request.FromDate,
                FromTime = request.FromTime,
                //ToDate = request.ToDate,
                //ToTime = request.ToTime,
                OrderBy = request.OrderBy,
                PageSize = request.PageSize,
            };

            return await _contactDetailQueryService.GetContactDetails(query);
        }

        private async Task UpdateLastDataJob(CreateTempFlatContactDetailListJobCommand request, FlatContactDetailResponse contactDetail, CancellationToken cancellationToken)
        {
            var lastJob = await _TempFlatDataJobQueryService.GetLastFlatData(new GetTempFlatDataJobLastQuery() { ReportType = (ReportType)((int)request.ContactReportType) });

            long result = 0;
            if (contactDetail != null && contactDetail.Date.Year != 2000)
            {
                var entity = new Goldiran.VOIPPanel.Domain.AggregatesModel.TempFlatDataJobs.TempFlatDataJob(true, contactDetail.Count, "", contactDetail.Date, (ReportType)((int)request.ContactReportType));

                _TempFlatDataJobRepository.Add(entity);
                //await _TempFlatDataJobRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            else if (contactDetail != null && contactDetail.Date.Year == 2000)
            {
                var entity = new Goldiran.VOIPPanel.Domain.AggregatesModel.TempFlatDataJobs.TempFlatDataJob(true, contactDetail.Count, "", lastJob != null? lastJob.LastDate.AddMinutes(30): new DateTime(2025, 5, 1, 0, 30, 0), (ReportType)((int)request.ContactReportType));

                _TempFlatDataJobRepository.Add(entity);
                //await _TempFlatDataJobRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }

        }

    }
}
