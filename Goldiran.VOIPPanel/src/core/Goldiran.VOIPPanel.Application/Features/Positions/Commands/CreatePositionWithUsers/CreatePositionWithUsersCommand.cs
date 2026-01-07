using AutoMapper;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using Voip.Framework.Domain;
using Azure;

namespace Goldiran.VOIPPanel.Application.Features.Positions.Commands.CreatePositionWithUsers;

public class CreatePositionWithUsersCommand : BaseCreateCommandRequest, IRequest<long>
{
    //public long DepartmentId { get; set; }
    public PositionType PositionTypeId { get; set; }
    public long? ParentPositionId { get; set; }
    public string Title { get; set; }
    public bool IsContentAccess { get; set; }
    public ShiftType? ShiftType { get; set; }
    public bool? HasShifts { get; set; }
    public List<AdedUserPosition> AdedUserPositionList { get; set; }
    public override bool IsTransactional { get; set; } = true;
    public class AdedUserPosition()
    {
        public long UserId { get;  set; }
        public string Queues { get;  set; }
        public string Extension { get;  set; }
        public string ServerIp { get;  set; }
    }

    public class Handler : IRequestHandler<CreatePositionWithUsersCommand, long>
    {
        private readonly IMapper _mapper;
        private readonly IPositionRepository _positionRepository;
        private readonly IUserPositionRepository _userPositionRepository;
        public Handler(IMapper mapper, IPositionRepository positionRepository, IUserPositionRepository userPositionRepository)
        {
            _mapper = mapper;
            _positionRepository = positionRepository;
            _userPositionRepository = userPositionRepository;
        }

        public async Task<long> Handle(CreatePositionWithUsersCommand request, CancellationToken cancellationToken)
        {

            Guid guid = Guid.NewGuid();
            Position parentPosition = null;
            if (request.ParentPositionId != null)
            {
                parentPosition = await _positionRepository.FindByIdAsync((long)request.ParentPositionId, new List<System.Linq.Expressions.Expression<Func<Position, object>>>() { c => c.ParentPosition });
            }

            var position = new Position(request.Title, request.PositionTypeId, request.ParentPositionId, (request.ParentPositionId != null) ? (parentPosition.ContactedParentPositionId + ">'" + request.ParentPositionId + "'") : null, (request.ParentPositionId != null) ? (parentPosition.ContactedParentPositionName + ">" + request.Title) : request.Title, request.IsContentAccess,request.ShiftType,request.HasShifts);

            _positionRepository.Add(position);
            await _positionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            var userPositionList = request.AdedUserPositionList.Select(c => new UserPosition(c.UserId,position.Id,DateTime.Now,null,null,true,c.Queues,c.Extension,c.ServerIp)).ToList();

            _userPositionRepository.AddRange(userPositionList);
            await _userPositionRepository.UnitOfWork.SaveChangesAsync( cancellationToken);


            return position.Id;
        }

    }
}

