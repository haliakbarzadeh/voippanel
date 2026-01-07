using AutoMapper;
using Goldiran.VOIPPanel.Application.Features.Positions.Commands.CreatePosition;
using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails;
using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails.Enums;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.Application.Features.FlatContactDetails.Commands.CreateFlatContactDetail;

public class CreateFlatContactDetailListCommand:BaseCreateCommandRequest, IRequest<long>
{
    public IList<CreateFlatContactDetailCommand> CreateFlatContactDetailCommands { get; set; }=new List<CreateFlatContactDetailCommand>();
    public class CreateFlatContactDetailCommand
    {
        public string LinkedId { get; set; } 
        public string DstChannel { get; set; } 
        public string Status { get; set; } 
        public string Disposition { get; set; }
        public DateTime Date { get; set; }
        public string Dcontext { get; set; } 
        public string? Source { get; set; }
        public string? Dest { get; set; }
        public int? Billsecond { get; set; }
        public int? Duration { get; set; }
        public int? Waiting { get; set; }
        public string? Filepath { get; set; }
        public string? Recordingfile { get; set; }
        public ReportType ReportType { get; set; }
        public string? QueueName { get; set; }

    }
    public class Handler : IRequestHandler<CreateFlatContactDetailListCommand, long>
    {
        private readonly IMapper _mapper;
        private readonly IContactDetailRepository _contactDetailRepository;
        public Handler(IMapper mapper, IContactDetailRepository contactDetailRepository)
        {
            _mapper = mapper;
            _contactDetailRepository = _contactDetailRepository;
        }

        public async Task<long> Handle(CreateFlatContactDetailListCommand request, CancellationToken cancellationToken)
        {
            var list=request.CreateFlatContactDetailCommands.Select(c=>new ContactDetail(c.LinkedId, c.DstChannel, c.Status, c.Disposition, c.Date, c.Dcontext, c.Source, c.Dest, c.Billsecond, c.Duration, c.Waiting, c.Filepath, c.Recordingfile,c.ReportType,c.QueueName,string.Empty,0,0,0,0)).ToList();

            _contactDetailRepository.AddRange(list);
            await _contactDetailRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return 1;
        }

    }
}
