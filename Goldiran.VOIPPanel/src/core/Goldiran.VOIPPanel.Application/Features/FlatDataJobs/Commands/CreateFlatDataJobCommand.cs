using AutoMapper;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using MediatR;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Voip.Framework.Common.Exceptions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.FlatDataJobs.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.FlatDataJobs;
using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails.Enums;

namespace Goldiran.VOIPPanel.Application.Features.Queus.Commands.CreateQueu;

public class CreateFlatDataJobCommand : BaseCreateCommandRequest, IRequest<long>
{
    public bool Status { get; set; }
    public int Count { get; set; }
    public string? Message { get; set; }
    public DateTime LastDate { get; set; }
    public ReportType ReportType { get; set; }
    public class Handler : IRequestHandler<CreateFlatDataJobCommand, long>
    {
        private readonly IFlatDataJobRepository _flatDataJobRepository;
        private readonly IReadModelContext _readModelContext;
        private IMapper _mapper;
        public Handler(IFlatDataJobRepository flatDataJobRepository, IReadModelContext readModelContext, IMapper mapper)
        {
            _flatDataJobRepository = flatDataJobRepository;
            _readModelContext = readModelContext;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreateFlatDataJobCommand request, CancellationToken cancellationToken)
        {
            var entity = new FlatDataJob(request.Status,request.Count,request.Message, request.LastDate,request.ReportType);

            _flatDataJobRepository.Add(entity);
            await _flatDataJobRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
           
            return entity.Id;
        }

    }
}

