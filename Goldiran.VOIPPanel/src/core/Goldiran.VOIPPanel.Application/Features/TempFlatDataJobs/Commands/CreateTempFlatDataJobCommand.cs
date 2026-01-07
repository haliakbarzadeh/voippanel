using AutoMapper;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using MediatR;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Voip.Framework.Common.Exceptions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.TempFlatDataJobs.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.TempFlatDataJobs;
using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails.Enums;

namespace Goldiran.VOIPPanel.Application.Features.Queus.Commands.CreateQueu;

public class CreateTempFlatDataJobCommand : BaseCreateCommandRequest, IRequest<long>
{
    public bool Status { get; set; }
    public int Count { get; set; }
    public string? Message { get; set; }
    public DateTime LastDate { get; set; }
    public ReportType ReportType { get; set; }
    public class Handler : IRequestHandler<CreateTempFlatDataJobCommand, long>
    {
        private readonly ITempFlatDataJobRepository _TempFlatDataJobRepository;
        private readonly IReadModelContext _readModelContext;
        private IMapper _mapper;
        public Handler(ITempFlatDataJobRepository TempFlatDataJobRepository, IReadModelContext readModelContext, IMapper mapper)
        {
            _TempFlatDataJobRepository = TempFlatDataJobRepository;
            _readModelContext = readModelContext;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreateTempFlatDataJobCommand request, CancellationToken cancellationToken)
        {
            var entity = new TempFlatDataJob(request.Status,request.Count,request.Message, request.LastDate,request.ReportType);

            _TempFlatDataJobRepository.Add(entity);
            await _TempFlatDataJobRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
           
            return entity.Id;
        }

    }
}

