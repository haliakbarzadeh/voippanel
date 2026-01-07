using AutoMapper;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Goldiran.VOIPPanel.Application.Features.Positions.Commands.CreatePosition;

public class CreatePositionCommand : BaseCreateCommandRequest, IRequest<long>
{
    //public long DepartmentId { get; set; }
    public PositionType PositionTypeId { get; set; }
    public long? ParentPositionId { get; set; }
    public string Title { get; set; }
    public bool IsContentAccess { get; set; }
    public ShiftType? ShiftType { get; set; }
    public bool? HasShifts { get; set; }
    public class Handler : IRequestHandler<CreatePositionCommand, long>
    {
        private readonly IMapper _mapper;
        private readonly IPositionRepository _positionRepository;
        public Handler(IMapper mapper, IPositionRepository positionRepository)
        {
            _mapper = mapper;
            _positionRepository = positionRepository;
        }

        public async Task<long> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
        {
            Guid guid = Guid.NewGuid();
            Position parentPosition = null;
            if (request.ParentPositionId != null)
            {
                parentPosition = await _positionRepository.FindByIdAsync((long)request.ParentPositionId, new List<System.Linq.Expressions.Expression<Func<Position, object>>>() { c=>c.ParentPosition});
            }

            var position = new Position(request.Title, request.PositionTypeId, request.ParentPositionId, (request.ParentPositionId != null) ? (parentPosition.ContactedParentPositionId + ">'" + request.ParentPositionId + "'") : null, (request.ParentPositionId != null) ? (parentPosition.ContactedParentPositionName + ">" + request.Title) : request.Title, request.IsContentAccess,request.ShiftType,request.HasShifts);

            _positionRepository.Add(position);
            await _positionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return position.Id;
        }

    }
}

