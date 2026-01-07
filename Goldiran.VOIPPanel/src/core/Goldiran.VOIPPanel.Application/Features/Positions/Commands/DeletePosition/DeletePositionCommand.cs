using AutoMapper;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Common.Exceptions;

namespace Goldiran.VOIPPanel.Application.Features.Positions.Commands.DeletePosition;

public class DeletePositionCommand : BaseCreateCommandRequest, IRequest<long>
{

    public long Id { get; set; }
    public class Handler : IRequestHandler<DeletePositionCommand,long>
    {
        private readonly IMapper _mapper;
        private readonly IPositionRepository _positionRepository;
        public Handler(IMapper mapper, IPositionRepository positionRepository)
        {
            _mapper = mapper;
            _positionRepository = positionRepository;
        }

        public async Task<long> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
        {

            var position = await _positionRepository.FindByIdAsync(request.Id);

            if (position == null)
            {
                throw new NotFoundException(nameof(Position), request.Id);
            }

            //if (Position.ApplicationRoles.Count > 0)
            //{
            //    throw new Exception("به دلیل وجود اطلاعات وابسته به این دپارتمان .امکان حذف وجود ندارد");
            //}

            _positionRepository.Delete(position);

            await _positionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return position.Id;
        }

    }
}



