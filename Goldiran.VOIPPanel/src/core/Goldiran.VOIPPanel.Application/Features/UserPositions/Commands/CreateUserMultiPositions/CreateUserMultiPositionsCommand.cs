using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Common.Models.CQRS.Command;
using Saramad.Core.ApplicationService.Features.MultiUserPositionss.Commands.CreateMultiUserPositions;
using Saramad.Core.Domain.Entities;
using Saramad.Core.Domain.Entities.AuditLog;
using Saramad.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saramad.Core.ApplicationService.Features.UserMultiPositionss.Commands.CreateUserMultiPositions;

    public class CreateUserMultiPositionsCommand : BaseCreateCommandRequest, IRequest<bool>
    {
    public long Userid { get; set; }
    public List<long> PositionId { get; set; }
    //public DateTime StartDate { get; set; }

    //public DateTime? EndDate { get; set; }
    //public DateTime? ExpireDate { get; set; }

    public class Handler : IRequestHandler<CreateUserMultiPositionsCommand, bool>
        {
            private readonly IApplicationDbContext _db;

            public Handler(IApplicationDbContext db)
            {
                _db = db;
            }

        public async Task<bool> Handle(CreateUserMultiPositionsCommand request, CancellationToken cancellationToken)
        {
            /////
            var currentUserIds = await _db.UserPositions.Where(C => C.UserId == request.Userid && C.EndDate == null).Select(c => c.PositionId).ToListAsync();
            if (currentUserIds == null)
            {
                currentUserIds = new List<long>();
            }
            var newRolesToAdd = _db.Positions.Where(c => request.PositionId.Except(currentUserIds).ToList().Contains(c.Id)).Select(c=>c.Id).ToList();
            _db.UserPositions.AddRange(newRolesToAdd.Select(c => new UserPosition() { PositionId = c, UserId = Convert.ToInt64(request.Userid), StartDate = DateTime.Now, IsActive = true }));

            var removedRoles = currentUserIds.Except(request.PositionId).ToList();
            var userRolesRemoved = _db.UserPositions.Where(c => c.UserId == request.Userid && removedRoles.Contains(c.PositionId));
            foreach (var item in userRolesRemoved)
            {
                item.EndDate = DateTime.Now;
                item.ExpireDate = DateTime.Now;
                item.IsActive = false;
            }
            _db.UserPositions.UpdateRange(userRolesRemoved);
            /////
            //var MultiUserPositions = request.PositionId.Select(c => new UserPosition()
            //{
            //    IsActive = true,
            //    UserId = request.UserId,
            //    PositionId = c,
            //    StartDate = DateTime.Now,
            //    //EndDate= request.EndDate,
            //    //ExpireDate= request.ExpireDate
            //}
            //  ).ToList();

            //await _db.UserPositions.AddRangeAsync(MultiUserPositions);
            await SetAudit(request, newRolesToAdd, removedRoles);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        
        }
        private async Task SetAudit(CreateUserMultiPositionsCommand request, List<long> addedList, List<long> removeList)
        {
            DateTime date = DateTime.Now;

            var addedAuditLogList = new AuditLog()
            {
                Guid = Guid.NewGuid(),
                UserId = request.UserId,
                UserIp = request.UserIp,
                ActionDate = date,
                AuditLogEntityType = AuditLogEntityTypeEnum.UserPosittionEntity,
                AuditLogFunctionType = AuditLogFunctionTypeEnum.AddRange,
                SourceValue = JsonConvert.SerializeObject(addedList),
                ObjectId = request.Userid.ToString()
            };

            var deletedAuditLogList = new AuditLog()
            {
                Guid = Guid.NewGuid(),
                UserId = request.UserId,
                UserIp = request.UserIp,
                ActionDate = date,
                AuditLogEntityType = AuditLogEntityTypeEnum.UserPosittionEntity,
                AuditLogFunctionType = AuditLogFunctionTypeEnum.Delete,
                SourceValue = JsonConvert.SerializeObject(removeList),
                ObjectId = request.Userid.ToString()
            };

            await _db.AuditLogs.AddAsync(addedAuditLogList);
            await _db.AuditLogs.AddAsync(deletedAuditLogList);
        }
    }
}

