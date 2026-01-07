using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;


//using ApiCallerGateway.ViewModels.Identity.Settings;

namespace Goldiran.VOIPPanel.Application.Common.Services.IdentityService
{
    public class UsedPasswordsService : IUsedPasswordsService
    {
        private readonly int _changePasswordReminderDays;
        private readonly int _notAllowedPreviouslyUsedPasswords;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IApplicationDbContext _db;

        public UsedPasswordsService(
            IPasswordHasher<AppUser> passwordHasher,
            IOptionsSnapshot<SiteSettings> configurationRoot,
            IApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));

            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            if (configurationRoot == null) throw new ArgumentNullException(nameof(configurationRoot));
            var configurationRootValue = configurationRoot.Value;
            if (configurationRootValue == null) throw new ArgumentNullException(nameof(configurationRootValue));
            _notAllowedPreviouslyUsedPasswords = configurationRootValue.NotAllowedPreviouslyUsedPasswords;
            _changePasswordReminderDays = configurationRootValue.ChangePasswordReminderDays;
        }

        public async Task AddToUsedPasswordsListAsync(AppUser user)
        {
            _db.AppUserUsedPasswords.Add(new AppUserUsedPassword
            {
                UserId = user.Id,
                HashedPassword = user.PasswordHash
            });
            await _db.SaveChangesAsync(new CancellationToken());
        }

        //public async Task<DateTime?> GetLastUserPasswordChangeDateAsync(int userId)
        //{
        //    var lastPasswordHistory =
        //        await _db.AppUserUsedPasswords//.AsNoTracking() --> removes shadow properties
        //                                .OrderByDescending(userUsedPassword => userUsedPassword.Id)
        //                                .FirstOrDefaultAsync(userUsedPassword => userUsedPassword.UserId == userId);
        //    if (lastPasswordHistory == null)
        //    {
        //        return null;
        //    }

        //    var createdDateValue =_db.GetShadowPropertyValue(lastPasswordHistory, AuditableShadowProperties.CreatedDateTime);
        //    return createdDateValue == null ?
        //              (DateTime?)null :
        //              DateTime.SpecifyKind((DateTime)createdDateValue, DateTimeKind.Utc);
        //}

        //public async Task<bool> IsLastUserPasswordTooOldAsync(int userId)
        //{
        //    var createdDateTime = await GetLastUserPasswordChangeDateAsync(userId);
        //    if (createdDateTime == null)
        //    {
        //        return false;
        //    }
        //    return createdDateTime.Value.AddDays(_changePasswordReminderDays) < DateTime.UtcNow;
        //}

        /// <summary>
        /// This method will be used by CustomPasswordValidator automatically,
        /// every time a user wants to change his/her info.
        /// </summary>
        public async Task<bool> IsPreviouslyUsedPasswordAsync(AppUser user, string newPassword)
        {
            if (user.Id == 0)
            {
                // A new user wants to register at our site
                return false;
            }

            var userId = user.Id;
            var usedPasswords = await _db.AppUserUsedPasswords
                                .AsNoTracking()
                                .Where(userUsedPassword => userUsedPassword.UserId == userId)
                                .OrderByDescending(userUsedPassword => userUsedPassword.Id)
                                .Select(userUsedPassword => userUsedPassword.HashedPassword)
                                .Take(_notAllowedPreviouslyUsedPasswords)
                                .ToListAsync();
            return usedPasswords.Any(hashedPassword => _passwordHasher.VerifyHashedPassword(user, hashedPassword, newPassword) != PasswordVerificationResult.Failed);
        }
    }
}