using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RPosition=Goldiran.VOIPPanel.ReadModel.Entities.Position;
using RUserPosition = Goldiran.VOIPPanel.ReadModel.Entities.UserPosition;
using RAppUser = Goldiran.VOIPPanel.ReadModel.Entities.AppUser;
using static Goldiran.VOIPPanel.Application.Features.Positions.Commands.CreatePositionWithUsers.CreatePositionWithUsersCommand;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;

namespace Goldiran.VOIPPanel.Application.Features.Positions.Commands.UpdatePositionWithUsers;

public class UpdatePositionWithUsersCommand : BaseCreateCommandRequest, IRequest<bool>
{
    public long Id { get; set; }
    public PositionType PositionTypeId { get; set; }
    public long? ParentPositionId { get; set; }
    public string Title { get; set; }
    public bool IsContentAccess { get; set; }
    public List<AdedUserPosition> AddedUserPositionList { get; set; }
    public override bool IsTransactional { get; set; } = true;
    public ShiftType? ShiftType { get; set; }
    public bool? HasShifts { get; set; }


    public class Handler : IRequestHandler<UpdatePositionWithUsersCommand,bool>
    {
        private IMapper _mapper;
        private readonly IPositionRepository _positionRepository;
        private readonly IUserPositionRepository _userPositionRepository;
        private readonly IReadModelContext _readModelContext;
        public Handler(IMapper mapper, IPositionRepository positionRepository, IUserPositionRepository userPositionRepository, IReadModelContext readModelContext)
        {
            _mapper = mapper;
            _positionRepository = positionRepository;
            _userPositionRepository = userPositionRepository;
            _readModelContext = readModelContext;
        }

        public async Task<bool> Handle(UpdatePositionWithUsersCommand request, CancellationToken cancellationToken)
        {

            var position = await _positionRepository.FindByIdAsync(request.Id);
            if (position == null)
            {
                throw new NotFoundException();
            }
            RPosition parentPosition = null;
            if (request.ParentPositionId != null)
            {
                parentPosition = await _readModelContext.Set<RPosition>().Where(c=>c.ParentPositionId==request.ParentPositionId).SingleOrDefaultAsync();
            }

            position.Update(request.Title, request.PositionTypeId, request.ParentPositionId, (request.ParentPositionId != null) ? (parentPosition.ContactedParentPositionId + ">'" + request.ParentPositionId + "'") : null, (request.ParentPositionId != null) ? (parentPosition.ContactedParentPositionName + ">" + parentPosition.Title) : request.Title, request.IsContentAccess,request.ShiftType,request.HasShifts);
            /////
            var currentUserIds =await _readModelContext.Set<RUserPosition>().Where(C => C.PositionId == request.PositionId && C.EndDate == null).Select(c => c.UserId).ToListAsync();
            if (currentUserIds == null)
            {
                currentUserIds = new List<long>();
            }
            var newUsersToAdd =_readModelContext.Set<RAppUser>().Where(c =>request.AddedUserPositionList.Select(c=>c.UserId).Except(currentUserIds).ToList().Contains(c.Id)).Select(c=>c.Id).ToList();
            _userPositionRepository.AddRange(newUsersToAdd.Select(c => new UserPosition(c, Convert.ToInt64(request.PositionId),DateTime.Now,null,null,true,request.AddedUserPositionList.FirstOrDefault(e=>e.UserId==c).Queues, request.AddedUserPositionList.FirstOrDefault(e => e.UserId == c).Extension, request.AddedUserPositionList.FirstOrDefault(e => e.UserId == c).ServerIp)).ToList());

            var removedUsers = currentUserIds.Except(request.AddedUserPositionList.Select(c=>c.UserId)).ToList();
            var usersRemoved =_userPositionRepository.GetAll(c => c.PositionId == request.PositionId && removedUsers.Contains(c.UserId));
            foreach (var item in usersRemoved)
            {
                item.SetDisactivePosition();
            }
            _userPositionRepository.UpdateRange(usersRemoved.ToList());

            var usersUpdated= _userPositionRepository.GetAll(c =>c.IsActive && c.PositionId == request.PositionId && currentUserIds.Except(removedUsers).Contains(c.UserId));
            foreach (var item in usersUpdated)
            {
                var userUpdatedRequest=request.AddedUserPositionList.FirstOrDefault(c=>c.UserId == item.UserId);
                item.Update(userUpdatedRequest.Queues,userUpdatedRequest.Extension,userUpdatedRequest.ServerIp);
            }
            _userPositionRepository.UpdateRange(usersUpdated.ToList());

            _positionRepository.Update(position);

            await _positionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        
    }
}
