
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;

namespace Goldiran.VOIPPanel.Application.Common.Services.IdentityService
{
    public interface IUsersPhotoService
    {
        string GetUsersAvatarsFolderPath();
        void SetUserDefaultPhoto(AppUser user);
        string GetUserDefaultPhoto(string photoFileName);
        string GetUserPhotoUrl(string photoFileName);
        string GetCurrentUserPhotoUrl();
    }
}