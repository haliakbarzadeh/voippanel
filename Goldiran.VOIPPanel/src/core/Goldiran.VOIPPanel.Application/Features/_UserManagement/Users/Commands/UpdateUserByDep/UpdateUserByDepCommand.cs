using AutoMapper;
using Framework.Common.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Saramad.Core.ApplicationService.Common.Exceptions;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Common.Models.AuditModels;
using Saramad.Core.ApplicationService.Common.Models.CQRS.Command;
using Saramad.Core.ApplicationService.Common.Services.IdentityService.IServices;
using Saramad.Core.ApplicationService.Features.Users.Commands.ActivateUser;
using Saramad.Core.ApplicationService.Features.Workflows.Commands.SetAnswerIncorrectWorkflowsCommand;
using Saramad.Core.Domain.Entities;
using Saramad.Core.Domain.Entities.AuditLog;
using Saramad.Core.Domain.Enums;
using System;

namespace Saramad.Core.ApplicationService.Features.Users.Commands.UpdateUserByDep;

public class UpdateUserByDepCommand : BaseUpdateCommandRequest, IRequest<bool>
{
    public long Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? NationalCode { get; set; }
    public string? PersianFullName { get; set; }
    public string? LatinFullName { get; set; }
    public int? OrganizationTypeId { get; set; }
    public int? DepartmentTypeId { get; set; }
    public string? NativeDepartmentName { get; set; }
    public string? NativePsitionName { get; set; }
    public string? Fax { get; set; }
    public string? Mobile { get; set; }
    public string? PostalCode { get; set; }
    public string? Address { get; set; }
    public string? ReportPath { get; set; }
    public List<Guid>? AttachmentFiles { get; set; }
    public class Handler : IRequestHandler<UpdateUserByDepCommand, bool>
    {
        private readonly IApplicationDbContext _db;
        private IMapper _mapper;
        private IApplicationUserManager _userManager;
        public Handler(IApplicationDbContext db, IMapper mapper, IApplicationUserManager userManager)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<bool> Handle(UpdateUserByDepCommand request, CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            await CheckPrivielage(request,(long)user.CreatedPositionId);

            await SetAudit(request, user, cancellationToken);
            user.ConvertModelToOtherModel(request);

            var result = await _userManager.UpdateAsync(user);



            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.Select(c => c.Description).ToList());
            }


            if (request.AttachmentFiles != null)
            {
                var removedfiles = _db.Files
              .Where(e => e.FileOwnerId == user.Guid
                          && e.FileOwnerTypeId == FileOwnerTypeEnum.User
                          && e.FileOwnerUsage == "attachment"
                          && !request.AttachmentFiles.Contains(e.Id));
                _db.Files.RemoveRange(removedfiles);

                var addFiles = _db.Files
                        .Where(e => e.FileOwnerTypeId == FileOwnerTypeEnum.User
                                    && e.FileOwnerUsage == "attachment"
                                    && request.AttachmentFiles.Contains(e.Id)
                                    && e.FileOwnerId == null);

                foreach (var file in addFiles)
                {
                    file.FileOwnerId = user.Guid;
                }
            }


            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }

        private async Task SetAudit(UpdateUserByDepCommand request, ApplicationUser user, CancellationToken cancellationToken)
        {
            var destValue = _mapper.Map<AuditUser>(request);
            destValue.IsActive = user.IsActive;
            destValue.IsTemperory = user.IsTemperory;

            var sourceValue = _mapper.Map<AuditUser>(user);

            var auditLog = new AuditLog()
            {
                Guid = Guid.NewGuid(),
                UserId = request.UserId,
                UserIp = request.UserIp,
                ActionDate = DateTime.Now,
                AuditLogEntityType = AuditLogEntityTypeEnum.UserEntity,
                AuditLogFunctionType = AuditLogFunctionTypeEnum.Update,
                SourceValue = JsonConvert.SerializeObject(sourceValue),
                DestinationValue = JsonConvert.SerializeObject(destValue),
                ObjectId = request.Id.ToString()
            };
            await _db.AuditLogs.AddAsync(auditLog);

        }

        private async Task CheckPrivielage(UpdateUserByDepCommand request, long posId)
        {
            if (!(request.PositionId == posId || request.PositionCildIds.Contains(posId)) && request.HasVerticalSecurity)
            {
                var privielage = await _db.MonitoredDepartmentsPrivileges.Include(c => c.DepartmentPrivilegeType).AsNoTracking().Where(c => c.DepartmentPrivilegeType == request.DepartmentPrivilegeType && c.MonitoredDepartments.SourceNodeId == request.PositionId && c.MonitoredDepartments.SourcePrivilegeNodeType == PrivilegeNodeTypeEnum.Position && c.MonitoredDepartments.DestNodeId == posId && c.MonitoredDepartments.DestPrivilegeNodeType == PrivilegeNodeTypeEnum.Position && (c.EnableModification != null && (bool)c.EnableModification)).FirstOrDefaultAsync();
                if (privielage == null)
                    throw new ValidationException(new List<string>() { "دسترسی برای انجام عملیات مورد نظر وجود ندارد" });
            }
        }
    }
}

