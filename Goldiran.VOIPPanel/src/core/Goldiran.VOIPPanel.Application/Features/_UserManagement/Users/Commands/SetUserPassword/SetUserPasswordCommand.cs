using AutoMapper;
using Framework.Common.SMS;
using MediatR;
using Newtonsoft.Json;
using Saramad.Core.ApplicationService.Common.Exceptions;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Common.Models.CQRS.Command;
using Saramad.Core.ApplicationService.Common.Services.IdentityService.IServices;
using Saramad.Core.ApplicationService.Features.Users.Commands.ActivateUser;
using Saramad.Core.Domain.Entities.AuditLog;
using Saramad.Core.Domain.Entities;
using Saramad.Core.Domain.Enums;
using Saramad.Core.ApplicationService.Common.Models.AuditModels;

namespace Saramad.Core.ApplicationService.Features.Users.Commands.SetUserPassword;

public class SetUserPasswordCommand : BaseUpdateCommandRequest, IRequest<bool>
{
    public long UserId { get; set; }
    //public string Password { get; set; }
    public bool IsActive { get; set; }


    public class Handler : IRequestHandler<SetUserPasswordCommand, bool>
    {
        private readonly IApplicationDbContext _db;
        private IMapper _mapper;
        private IApplicationUserManager _userManager;
        private ISMSSender _smsSender;
        public Handler(IApplicationDbContext db, IMapper mapper, IApplicationUserManager userManager, ISMSSender smsSender)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            _smsSender = smsSender;
        }

        public async Task<bool> Handle(SetUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var password = new Random().Next(10000, 99999).ToString();
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
                throw new InvalidOperationException("کاربر مورد نظر یافت نشد");


            //var test=await _userManager.RemovePasswordAsync(user);
            //var hasPassword = await _userManager.HasPasswordAsync(user);

            //if (hasPassword)
            //    throw new InvalidOperationException("برای این کاربر پیش از این رمز عبور تعیین شده است");

            //var result = await _userManager.AddPasswordAsync(user, password);
            //if (!result.Succeeded)
            //{
            //    throw new ValidationException(result.Errors.Select(c => c.Description).ToList());
            //}
            ////user.IsTemperory = false;
            ////result=await _userManager.UpdateAsync(user);
            ////if (!result.Succeeded)
            ////{
            ////    throw new ValidationException(result.Errors.Select(c => c.Description).ToList());
            ////}
            //await _smsSender.SendSms(user.PhoneNumber, $"رمز عبور موقت شما عبارت است از :{password}", null);
            user.IsActive = request.IsActive;
            await SetAudit(request, user, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }

        private async Task SetAudit(SetUserPasswordCommand request, ApplicationUser user, CancellationToken cancellationToken)
        {
            var destValue = _mapper.Map<AuditUser>(user);
            destValue.IsActive = request.IsActive;

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
                ObjectId = user.Id.ToString()
            };
            await _db.AuditLogs.AddAsync(auditLog);
        }
    }
}

