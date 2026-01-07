using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
namespace Goldiran.VOIPPanel.Application.Common.Services.IdentityService
{
    public interface ISiteStatService
    {
        Task<List<AppUser>> GetOnlineUsersListAsync(int numbersToTake, int minutesToTake);

        Task<List<AppUser>> GetTodayBirthdayListAsync();

        Task UpdateUserLastVisitDateTimeAsync(ClaimsPrincipal claimsPrincipal);

        //Task<AgeStatViewModel> GetUsersAverageAge();
    }
}