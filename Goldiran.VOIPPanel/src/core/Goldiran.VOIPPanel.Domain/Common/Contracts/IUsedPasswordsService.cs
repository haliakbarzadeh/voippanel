
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using System;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.Common.Contracts;

public interface IUsedPasswordsService
{
    Task<bool> IsPreviouslyUsedPasswordAsync(AppUser user, string newPassword);
    Task AddToUsedPasswordsListAsync(AppUser user);
    //Task<bool> IsLastUserPasswordTooOldAsync(long userId);
    //Task<DateTime?> GetLastUserPasswordChangeDateAsync(long userId);
}