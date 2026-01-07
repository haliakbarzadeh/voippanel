using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Extensions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore.Extensions;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Goldiran.VOIPPanel.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Persistence.Common.UserManagement.Users
{
    /// <summary>
    /// </summary>
    public class AppUserManager :
        UserManager<AppUser>,
        IAppUserManager
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly VOIPPanelContext _uow;
        //private readonly IUsedPasswordsService _usedPasswordsService;
        private readonly IdentityErrorDescriber _errors;
        private readonly ILookupNormalizer _keyNormalizer;
        private readonly ILogger<AppUserManager> _logger;
        private readonly IOptions<IdentityOptions> _optionsAccessor;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IEnumerable<IPasswordValidator<AppUser>> _passwordValidators;
        private readonly IServiceProvider _services;
        private readonly DbSet<AppUser> _users;
        private readonly DbSet<AppRole> _roles;
        private readonly IAppUserStore _userStore;
        private readonly IMapper _mapper;
        private readonly IEnumerable<IUserValidator<AppUser>> _userValidators;
        private AppUser _currentUserInScope;

        public AppUserManager(
            IMapper mapper,
            IAppUserStore store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<AppUser> passwordHasher,
            IEnumerable<IUserValidator<AppUser>> userValidators,
            IEnumerable<IPasswordValidator<AppUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<AppUserManager> logger,
            IHttpContextAccessor contextAccessor,
            VOIPPanelContext uow)
            ////IUsedPasswordsService usedPasswordsService)
            : base(
                (UserStore<AppUser, AppRole, VOIPPanelContext, long, AppUserClaim, AppUserRole, AppUserLogin, AppUserToken, AppRoleClaim>)store,
                  optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _userStore = store ?? throw new ArgumentNullException(nameof(_userStore));
            _optionsAccessor = optionsAccessor ?? throw new ArgumentNullException(nameof(_optionsAccessor));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(_passwordHasher));
            _userValidators = userValidators ?? throw new ArgumentNullException(nameof(_userValidators));
            _passwordValidators = passwordValidators ?? throw new ArgumentNullException(nameof(_passwordValidators));
            _keyNormalizer = keyNormalizer ?? throw new ArgumentNullException(nameof(_keyNormalizer));
            _errors = errors ?? throw new ArgumentNullException(nameof(_errors));
            _services = services ?? throw new ArgumentNullException(nameof(_services));
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(_contextAccessor));
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            //_usedPasswordsService = usedPasswordsService ?? throw new ArgumentNullException(nameof(_usedPasswordsService));
            _users = uow.Users;
            _roles = uow.Roles;

        }

        #region BaseClass

        string IAppUserManager.CreateTwoFactorRecoveryCode()
        {
            return base.CreateTwoFactorRecoveryCode();
        }

        Task<PasswordVerificationResult> IAppUserManager.VerifyPasswordAsync(IUserPasswordStore<AppUser> store, AppUser user, string password)
        {
            return base.VerifyPasswordAsync(store, user, password);
        }

        public override async Task<IdentityResult> CreateAsync(AppUser user)
        {
            var result = await base.CreateAsync(user);
            //if (result.Succeeded)
            //{
            //    await _usedPasswordsService.AddToUsedPasswordsListAsync(user);
            //}
            return result;
        }

        public override async Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            var result = await base.CreateAsync(user, password);
            //if (result.Succeeded)
            //{
            //    await _usedPasswordsService.AddToUsedPasswordsListAsync(user);
            //}
            return result;
        }

        public override async Task<IdentityResult> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword)
        {
            var result = await base.ChangePasswordAsync(user, currentPassword, newPassword);
            //if (result.Succeeded)
            //{
            //    await _usedPasswordsService.AddToUsedPasswordsListAsync(user);
            //}
            return result;
        }

        public override async Task<IdentityResult> ResetPasswordAsync(AppUser user, string token, string newPassword)
        {
            var result = await base.ResetPasswordAsync(user, token, newPassword);
            //if (result.Succeeded)
            //{
            //    await _usedPasswordsService.AddToUsedPasswordsListAsync(user);
            //}
            return result;
        }

        public override async Task<AppUser> FindByNameAsync(string userName)
        {
            //return await _uow.Users.Include(c=>c.UserPositions).ThenInclude(c=>c.Position).ThenInclude(c=>c.Department).Where(c => c.UserName == userName).Include(c=>c.UserPositions).FirstOrDefaultAsync();
            return await _uow.Users.Where(c => c.UserName == userName).FirstOrDefaultAsync();

        }
        public async Task<AppUser> FindByUserIdAsync(long UserId)
        {
            //return await _uow.AppUsers.Include(c => c.UserPositions).ThenInclude(c => c.Position).ThenInclude(c => c.Department).Where(c => c.Id == UserId).Include(c => c.UserPositions).FirstOrDefaultAsync();
            return await _uow.Users.Where(c => c.Id == UserId).FirstOrDefaultAsync();

        }

        #endregion

        #region CustomMethods
        //public User FindById(int userId, bool claimInclude)
        //{
        //    if (claimInclude)
        //    {
        //        return _users.Where(c => c.Id == userId).Include(c => c.Claims).First();
        //    }
        //    else
        //    {
        //        return _users.Find(userId);
        //    }

        //}
        public AppUser FindById(long userId)
        {
            return _users.Find(userId);

        }

        public async Task<AppUser> FindByIdIncludeUserRolesAsync(long userId)
        {
            return await _users.Include(x => x.Roles).Include(d => d.Claims).FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<List<AppUser>> GetAllUsersAsync()
        {
            return await Users.ToListAsync();
        }

        public async Task<AppUser> GetUserById(long userId)
        {
            if (_currentUserInScope != null)
            {
                return _currentUserInScope;
            }


            return _currentUserInScope = await FindByIdIncludeUserRolesAsync(userId);
        }
        public async Task<AppUser> GetCurrentUser()
        {
            if (_currentUserInScope != null)
            {
                return _currentUserInScope;
            }

            var currentUserId = GetCurrentUserId();
            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return null;
            }

            var userId = int.Parse(currentUserId);
            return _currentUserInScope = await FindByIdIncludeUserRolesAsync(userId);
        }

        public async Task<AppUser> GetCurrentUserAsync()
        {
            return _currentUserInScope ??
                (_currentUserInScope = await GetUserAsync(_contextAccessor.HttpContext.User));
        }

        public string GetCurrentUserId()
        {
            return _contextAccessor.HttpContext.User.Identity.GetUserId();
        }

        public long? CurrentUserId
        {
            get
            {
                var userId = _contextAccessor.HttpContext.User.Identity.GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return null;
                }

                return !long.TryParse(userId, out long result) ? (int?)null : result;
            }
        }

        IPasswordHasher<AppUser> IAppUserManager.PasswordHasher { get => base.PasswordHasher; set => base.PasswordHasher = value; }

        IList<IUserValidator<AppUser>> IAppUserManager.UserValidators => base.UserValidators;

        IList<IPasswordValidator<AppUser>> IAppUserManager.PasswordValidators => base.PasswordValidators;

        IQueryable<AppUser> IAppUserManager.Users => base.Users;

        public string GetCurrentUserName()
        {
            return _contextAccessor.HttpContext.User.Identity.GetUserName();
        }

        public async Task<bool> HasPasswordAsync(long userId)
        {
            var user = await FindByIdAsync(userId.ToString());
            return user?.PasswordHash != null;
        }

        public async Task<bool> HasPhoneNumberAsync(long userId)
        {
            var user = await FindByIdAsync(userId.ToString());
            return user?.PhoneNumber != null;
        }

        //public async Task<byte[]> GetEmailImageAsync(int? userId)
        //{
        //    if (userId == null)
        //        return "?".TextToImage(new TextToImageOptions());

        //    var user = await FindByIdAsync(userId.Value.ToString());
        //    if (user == null)
        //        return "?".TextToImage(new TextToImageOptions());

        //    if (!user.IsEmailPublic)
        //        return "?".TextToImage(new TextToImageOptions());

        //    return user.Email.TextToImage(new TextToImageOptions());
        //}

        //public async Task<PagedUsersListViewModel> GetPagedUsersListAsync(SearchUsersViewModel model, int pageNumber)
        //{
        //    var skipRecords = pageNumber * model.MaxNumberOfRows;
        //    var query = _users.Include(x => x.Roles).AsNoTracking();

        //    if (!model.ShowAllUsers)
        //    {
        //        query = query.Where(x => x.IsActive == model.UserIsActive);
        //    }

        //    if (!string.IsNullOrWhiteSpace(model.TextToFind))
        //    {
        //        model.TextToFind = model.TextToFind.ApplyCorrectYeKe();

        //        if (model.IsPartOfEmail)
        //        {
        //            query = query.Where(x => x.Email.Contains(model.TextToFind));
        //        }

        //        if (model.IsUserId)
        //        {
        //            if (int.TryParse(model.TextToFind, out int userId))
        //            {
        //                query = query.Where(x => x.Id == userId);
        //            }
        //        }

        //        if (model.IsPartOfName)
        //        {
        //            query = query.Where(x => x.FirstName.Contains(model.TextToFind));
        //        }

        //        if (model.IsPartOfLastName)
        //        {
        //            query = query.Where(x => x.LastName.Contains(model.TextToFind));
        //        }

        //        if (model.IsPartOfUserName)
        //        {
        //            query = query.Where(x => x.UserName.Contains(model.TextToFind));
        //        }

        //        if (model.IsPartOfLocation)
        //        {
        //            query = query.Where(x => x.Location.Contains(model.TextToFind));
        //        }
        //    }

        //    if (model.HasEmailConfirmed)
        //    {
        //        query = query.Where(x => x.EmailConfirmed);
        //    }

        //    if (model.UserIsLockedOut)
        //    {
        //        query = query.Where(x => x.LockoutEnd != null);
        //    }

        //    if (model.HasTwoFactorEnabled)
        //    {
        //        query = query.Where(x => x.TwoFactorEnabled);
        //    }

        //    query = query.OrderBy(x => x.Id);
        //    return new PagedUsersListViewModel
        //    {
        //        Paging =
        //        {
        //            TotalItems = await query.CountAsync()
        //        },
        //        Users = await query.Skip(skipRecords).Take(model.MaxNumberOfRows).ToListAsync(),
        //        Roles = await _roles.ToListAsync()
        //    };
        //}

        //public async Task<PagedUsersListViewModel> GetPagedUsersListAsync(
        //    int pageNumber, int recordsPerPage,
        //    string sortByField, SortOrder sortOrder,
        //    bool showAllUsers)
        //{
        //    var skipRecords = pageNumber * recordsPerPage;
        //    var query = _users.Include(x => x.Roles).AsNoTracking();

        //    if (!showAllUsers)
        //    {
        //        query = query.Where(x => x.IsActive);
        //    }

        //    switch (sortByField)
        //    {
        //        case nameof(User.Id):
        //            query = sortOrder == SortOrder.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
        //            break;
        //        default:
        //            query = sortOrder == SortOrder.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
        //            break;
        //    }

        //    return new PagedUsersListViewModel
        //    {
        //        Paging =
        //        {
        //            TotalItems = await query.CountAsync()
        //        },
        //        Users = await query.Skip(skipRecords).Take(recordsPerPage).ToListAsync(),
        //        Roles = await _roles.ToListAsync()
        //    };
        //}

        public async Task<IdentityResult> UpdateUserAndSecurityStampAsync(long userId, Action<AppUser> action)
        {
            var user = await FindByIdIncludeUserRolesAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "کاربر مورد نظر یافت نشد."
                });
            }

            action(user);

            var result = await UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }
            return await UpdateSecurityStampAsync(user);
        }

        public async Task<IdentityResult> AddOrUpdateUserRolesAsync(long userId, IList<long> selectedRoleIds, Action<AppUser> action = null)
        {
            var currentUserRoleIds = await _uow.UserRoles.Where(C => C.UserId == userId).Select(c => c.RoleId).ToListAsync();
            if (selectedRoleIds == null)
            {
                selectedRoleIds = new List<long>();
            }
            var newRolesToAdd = selectedRoleIds.Except(currentUserRoleIds).ToList();
            _uow.UserRoles.AddRange(newRolesToAdd.Select(c => new AppUserRole() { UserId = userId, RoleId = c }));

            var removedRoles = currentUserRoleIds.Except(selectedRoleIds).ToList();
            var userRolesRemoved = _uow.UserRoles.Where(c => c.UserId == userId && removedRoles.Contains(c.RoleId));
            _uow.UserRoles.RemoveRange(userRolesRemoved);

            await _uow.SaveChangesAsync(new CancellationToken());
            return IdentityResult.Success;
        }



        Task<IdentityResult> IAppUserManager.UpdatePasswordHash(AppUser user, string newPassword, bool validatePassword)
        {
            return base.UpdatePasswordHash(user, newPassword, validatePassword);
        }

        //public async Task<PagedUsersListResponse> GetPagedUsersListAsync(GetUsersByFilterQuery model)
        //{
        //    var skipRecords = (model.PageNumber != null && model.PageSize != 0) ? (model.PageNumber - 1) * model.PageSize : null;
        //    var query = _users.Include(x => x.Roles).Include(c => c.Claims).AsNoTracking();

        //    if (model.UserIsActive != null)
        //    {
        //        query = query.Where(x => x.IsActive == model.UserIsActive);
        //    }

        //    if (model.Fk_TMSId != null)
        //    {
        //        query = query.Where(x => x.Fk_TMSId == model.Fk_TMSId);
        //    }

        //    if (model.Fk_TMSCoWorkerId != null)
        //    {
        //        query = query.Where(x => x.Fk_TMSCoWorkerId == model.Fk_TMSCoWorkerId);
        //    }

        //    if (!string.IsNullOrWhiteSpace(model.TextToFind))
        //    {
        //        model.TextToFind = model.TextToFind.ApplyCorrectYeKe();

        //        if (model.IsPartOfEmail != null && (bool)model.IsPartOfEmail)
        //        {
        //            query = query.Where(x => x.Email.Contains(model.TextToFind));
        //        }

        //        if (model.IsUserId != null && (bool)model.IsUserId)
        //        {
        //            if (int.TryParse(model.TextToFind, out int userId))
        //            {
        //                query = query.Where(x => x.Id == userId);
        //            }
        //        }

        //        if (model.IsPartOfName != null && (bool)model.IsPartOfName)
        //        {
        //            query = query.Where(x => x.FirstName.Contains(model.TextToFind));
        //        }

        //        if (model.IsPartOfLastName != null && (bool)model.IsPartOfLastName)
        //        {
        //            query = query.Where(x => x.LastName.Contains(model.TextToFind));
        //        }

        //        if (model.IsPartOfUserName != null && (bool)model.IsPartOfUserName)
        //        {
        //            query = query.Where(x => x.UserName.Contains(model.TextToFind));
        //        }

        //        //if (model.IsPartOfLocation != null && (bool)model.IsPartOfLocation)
        //        //{
        //        //    query = query.Where(x => x.Location.Contains(model.TextToFind));
        //        //}
        //    }

        //    //if (model.HasEmailConfirmed != null && (bool)model.HasEmailConfirmed)
        //    //{
        //    //    query = query.Where(x => x.EmailConfirmed);
        //    //}

        //    //if (model.UserIsLockedOut != null && (bool)model.UserIsLockedOut)
        //    //{
        //    //    query = query.Where(x => x.LockoutEnd != null);
        //    //}

        //    //if (model.HasTwoFactorEnabled != null && (bool)model.HasTwoFactorEnabled)
        //    //{
        //    //    query = query.Where(x => x.TwoFactorEnabled);
        //    //}
        //    var count = query.Count();
        //    query = query.OrderBy(x => x.Id);

        //    var users = (count != 0 ?( (model.PageNumber != null && model.PageSize!=0)? await query.Skip(Convert.ToInt32(skipRecords)).Take(Convert.ToInt32(model.PageSize)).ToListAsync(): await query.ToListAsync()) : null);
        //    var usersDto = (count != 0 ? users.Select(c => _mapper.Map<UserDto>(c)).ToList() : null);

        //    return new PagedUsersListResponse()
        //    {
        //        Users = usersDto,
        //        Count = count
        //    };

        //}

        public async Task<IdentityResult> AddClaimAsync(long userId, Claim claim)
        {
            var user = FindById(userId);
            return await base.AddClaimAsync(user, claim);
        }

        public async Task<IdentityResult> AddClaimsAsync(long userId, IEnumerable<Claim> claims)
        {
            var user = _users.Where(c => c.Id == userId).Include(c => c.Claims).First();
            var deletedClaims = await RemoveClaimsAsync(userId, user.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)));
            return await base.AddClaimsAsync(user, claims);
        }

        public async Task<IdentityResult> RemoveClaimAsync(long userId, Claim claim)
        {
            var user = FindById(userId);
            return await base.RemoveClaimAsync(user, claim);
        }

        public async Task<IdentityResult> RemoveClaimsAsync(long userId, IEnumerable<Claim> claims)
        {
            var user = FindById(userId);
            return await base.RemoveClaimsAsync(user, claims);
        }

        #endregion
    }
}