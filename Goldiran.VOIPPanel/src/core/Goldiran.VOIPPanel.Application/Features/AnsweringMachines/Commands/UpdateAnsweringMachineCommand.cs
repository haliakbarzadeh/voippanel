using AutoMapper;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus;
using MediatR;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Voip.Framework.Common.Exceptions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.AnsweringMachines.Enums;
using Goldiran.VOIPPanel.Domain.AggregatesModel.AnsweringMachines.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.AnsweringMachines.Models;

namespace Goldiran.VOIPPanel.Application.Features.AnsweringMachines.Commands;

public class UpdateAnsweringMachineCommand : BaseCreateCommandRequest, IRequest<bool>
{
    public long Id { get; set; }
    public AMStatusType Status { get; set; }
    public string Description { get; set; } = string.Empty;
    //public DateTime EditDate { get; set; }
    //public TimeSpan EditTime { get; set; }
    //public DateTime UpdateTime { get; set; }


    public class Handler : IRequestHandler<UpdateAnsweringMachineCommand, bool>
    {
        private readonly IAnsweringMachineRepository _answeringMachineRepository;

        public Handler(IAnsweringMachineRepository answeringMachineRepository)
        {
            _answeringMachineRepository = answeringMachineRepository;

        }

        public async Task<bool> Handle(UpdateAnsweringMachineCommand request, CancellationToken cancellationToken)
        {
            var date=DateTime.Now;
            var entity = new AnsweringMachineRequest()
            {
                Id=request.Id,
                Status = request.Status,
                Description = request.Description,
                Agent=(int)request.UserId,
                EditDate=date.Date,
                EditTime=date.TimeOfDay,
                UpdateTime=date
            };
                       
            var result=await _answeringMachineRepository.Update(entity);
           
            return result;
        }

    }
}

