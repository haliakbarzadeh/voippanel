using AutoMapper;
using Goldiran.Framework.Domain.Models.CQRS;
using Goldiran.Framework.EFCore;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Goldiran.VOIPPanel.Application.Features.UserPositions.Commands.CreateMultiUserPositions;

public class CreateMultiUserPositionsCommand : BaseCreateCommandRequest, IRequest<bool>
{
    public List<long> UserIdList { get; set; }
    public long PositionId { get; set; }
    //public DateTime StartDate { get; set; }

    //public DateTime? EndDate { get; set; }
    //public DateTime? ExpireDate { get; set; }

    public class Handler : IRequestHandler<CreateMultiUserPositionsCommand, bool>
    {
        private readonly IUserPositionRepository _userPositionRepository;
        private readonly IReadModelContext _readModelContext;
        private readonly IAppUserManager _appUserManager;
        private IMapper _mapper;
        public Handler( IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateMultiUserPositionsCommand request, CancellationToken cancellationToken)
        {
            /////
            var currentUserIds = await _db.UserPositions.Where(C => C.PositionId == request.PositionId && C.EndDate == null && C.IsActive).Select(c => c.UserId).ToListAsync();
            if (currentUserIds == null)
            {
                currentUserIds = new List<long>();
            }
            var newRolesToAdd = _db.ApplicationUsers.Where(c => request.UserIdList.Except(currentUserIds).ToList().Contains(c.Id)).Select(c => c.Id).ToList();
            _db.UserPositions.AddRange(newRolesToAdd.Select(c => new UserPosition() { UserId = c, PositionId = Convert.ToInt64(request.PositionId), StartDate = DateTime.Now, IsActive = true }));

            var removedRoles = currentUserIds.Except(request.UserIdList).ToList();
            var userRolesRemoved = _db.UserPositions.Where(c => c.PositionId == request.PositionId && removedRoles.Contains(c.UserId)).ToList();
            foreach (var item in userRolesRemoved)
            {
                item.EndDate = DateTime.Now;
                item.ExpireDate = DateTime.Now;
                item.IsActive = false;
            }
            _db.UserPositions.UpdateRange(userRolesRemoved);
            /////
            //var MultiUserPositions = request.UserId.Select(c=>new UserPosition()
            //    {
            //        IsActive= true,
            //        UserId= c,
            //        PositionId= request.PositionId,
            //        StartDate= DateTime.Now,
            //        //EndDate= request.EndDate,
            //        //ExpireDate= request.ExpireDate
            //    }
            //    ).ToList();

            //    await _db.UserPositions.AddRangeAsync(MultiUserPositions);

            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }

       
    }
}

