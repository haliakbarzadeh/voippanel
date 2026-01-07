
using AutoMapper;
using Goldiran.VOIPPanel.Application.Features.FlatContactDetails.Models;
using Goldiran.VOIPPanel.Application.Features.FlatContactDetails.Module;
using Goldiran.VOIPPanel.Application.Features.Positions.Commands.CreatePosition;
using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails;
using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails.Enums;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.FlatDataJobs.Contracts;
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

namespace Goldiran.VOIPPanel.Application.Features.FlatContactDetails.Commands.CreateFlatContactDetail;

public class CreateFlatAutoDialListJobCommand : BaseCreateCommandRequest, IRequest<FlatContactDetailResponse>
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
    public class Handler : IRequestHandler<CreateFlatAutoDialListJobCommand, FlatContactDetailResponse>
    {
        private readonly IMapper _mapper;
        private readonly IContactDetailRepository _contactDetailRepository;
        private readonly IContactDetailQueryService _contactDetailQueryService;
        private readonly IReadModelContext _readModelContext;
        private readonly IFlatDataJobQueryService _flatDataJobQueryService;
        private readonly IFlatDataJobRepository _flatDataJobRepository;


        public Handler(IMapper mapper, IContactDetailRepository contactDetailRepository, IContactDetailQueryService contactDetailQueryService, IFlatDataJobQueryService flatDataJobQueryService, IFlatDataJobRepository flatDataJobRepository, IReadModelContext readModelContext)
        {
            _mapper = mapper;
            _contactDetailRepository = contactDetailRepository;
            _contactDetailQueryService = contactDetailQueryService;
            _readModelContext = readModelContext;
            _flatDataJobQueryService = flatDataJobQueryService;
            _flatDataJobRepository = flatDataJobRepository;
        }

        public async Task<FlatContactDetailResponse> Handle(CreateFlatAutoDialListJobCommand request, CancellationToken cancellationToken)
        {
            var list = await GetContactDetails(request, cancellationToken);
            if (list.Items.Count == 0)
            {
                var notResult = new FlatContactDetailResponse() { Date = new DateTime(2000, 1, 1), Count = 0 };
                await UpdateLastDataJob(request, notResult, cancellationToken);
                return notResult;
            }

            var existList = _mapper.Map<List<AutoDialDto>>(_readModelContext.Set<Goldiran.VOIPPanel.ReadModel.Entities.ContactDetail>().AsNoTracking().OrderByDescending(c => c.Id).Take(30).ToList());

            List<AutoDialDto> addedList = new List<AutoDialDto>();
            if (request.ContactReportType == (ContactReportType)((int)ReportType.Normal))
                addedList = list.Items.Except(existList, new FlatNewAutoDialComparer()).ToList();
            else if (request.ContactReportType == (ContactReportType)((int)ReportType.AutoDial))
                addedList = list.Items.Except(existList, new FlatNewAutoDialComparer()).ToList();
            else
                addedList = list.Items.ToList();

            addedList = addedList.Where(c => c.Dest.Length < 50).ToList();

            if (addedList.IsNullOrEmpty()) { return new FlatContactDetailResponse() { Date = new DateTime(2000, 1, 1), Count = 0 }; }
            //var command=list.Items.Select(c=>new ContactDetail(c.LinkedId, c.DstChannel, c.Status, c.Disposition, c.Date, c.Dcontext, c.Source, c.Dest, c.Billsecond, c.Duration, c.Waiting, c.Filepath, c.Recordingfile,(ReportType)((int)request.ContactReportType))).OrderBy(c=>c.Date).ToList();
            var command = addedList.Select(c => new ContactDetail(c.LinkedId, c.DstChannel, c.Status, c.RealDisposition, c.Date, c.Dcontext, c.Source, c.Dest, c.Billsecond, c.Duration, 0, c.Filepath, c.Recordingfile, (ReportType)((int)request.ContactReportType), c.QueueName,c.CustomerStatus,c.CustomWaiting,c.AgentWaiting,c.CustomToAgentWaiting,c.AgentToCustomWaiting)).OrderBy(c => c.Date).ToList();

            var result = new FlatContactDetailResponse() { Date = command.LastOrDefault().Date, Count = list.Items.Count };

            await UpdateLastDataJob(request, result, cancellationToken);

            _contactDetailRepository.AddRange(command);
            await _contactDetailRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }

        private async Task<PaginatedList<AutoDialDto>> GetContactDetails(CreateFlatAutoDialListJobCommand request, CancellationToken cancellationToken)
        {
            var query = new GetAutoDetailsQuery()
            {
                IsRestricted = request.IsRestricted,
                IsJob = request.IsJob,
                ContactReportType = request.ContactReportType,
                FromDate = request.FromDate,
                FromTime = request.FromTime,
                ToDate = request.ToDate,
                ToTime = request.ToTime,
                OrderBy = request.OrderBy,
                PageSize = request.PageSize,
            };

            return await _contactDetailQueryService.GetAutoDetails(query);
        }

        private async Task UpdateLastDataJob(CreateFlatAutoDialListJobCommand request, FlatContactDetailResponse contactDetail, CancellationToken cancellationToken)
        {
            var lastJob = await _flatDataJobQueryService.GetLastFlatData(new GetFlatDataJobLastQuery() { ReportType = (ReportType)((int)request.ContactReportType) });

            long result = 0;
            if (contactDetail != null && contactDetail.Date.Year != 2000)
            {
                var entity = new Goldiran.VOIPPanel.Domain.AggregatesModel.FlatDataJobs.FlatDataJob(true, contactDetail.Count, "", contactDetail.Date, (ReportType)((int)request.ContactReportType));

                _flatDataJobRepository.Add(entity);
                //await _flatDataJobRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            else if (contactDetail != null && contactDetail.Date.Year == 2000)
            {
                var entity = new Goldiran.VOIPPanel.Domain.AggregatesModel.FlatDataJobs.FlatDataJob(true, contactDetail.Count, "", lastJob != null ? lastJob.LastDate.AddMinutes(30) : new DateTime(2025, 3, 21, 0, 30, 0), (ReportType)((int)request.ContactReportType));

                _flatDataJobRepository.Add(entity);
                //await _flatDataJobRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }

        }

    }
}
