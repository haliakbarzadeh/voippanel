using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Tokens;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Tokens.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserPosition = Goldiran.VOIPPanel.ReadModel.Entities.UserPosition;
using Goldiran.VOIPPanel.ReadModel.Mappers;
using Voip.Framework.Common.AppSettings;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Cube.UserManagementService.Domain.AggregatesModel.UserPositions.Contracts;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Common.Services.IdentityService
{
    public class TokenService : ITokenService
    {
        private readonly IOptionsSnapshot<SiteSettings> _siteOptions;
        private readonly IUserClaimsPrincipalFactory<Domain.Common.Entities.UserManagement.AppUser> _applicationClaimsPrincipalFactory;
        private readonly IUserPositionQueryService _userPositionQueryService;
        private readonly IPositionQueryService _positionQueryService;
        private readonly ITokenRepository _tokenRepository;
        private readonly IReadModelContext _context;
        private readonly IAppRoleManager _appRoleManager;
        private readonly IRoleQueryService _roleQueryService;
        private readonly IMapper _mapper;
        public TokenService(IOptionsSnapshot<SiteSettings> siteOptions, IUserClaimsPrincipalFactory<Domain.Common.Entities.UserManagement.AppUser> applicationClaimsPrincipalFactory, IUserPositionQueryService userPositionQueryService, IPositionQueryService positionQueryService, ITokenRepository tokenRepository, IReadModelContext context, IAppRoleManager appRoleManager, IMapper mapper, IRoleQueryService roleQueryService)
        {
            _siteOptions = siteOptions ?? throw new ArgumentNullException(nameof(siteOptions));
            _applicationClaimsPrincipalFactory = applicationClaimsPrincipalFactory;
            _userPositionQueryService = userPositionQueryService;
            _positionQueryService = positionQueryService;
            _tokenRepository = tokenRepository;
            _context = context;
            _appRoleManager = appRoleManager;
            _roleQueryService = roleQueryService;
            _mapper = mapper;
        }

        public async Task<Tuple<string, string>> GenerateAccessToken(Domain.Common.Entities.UserManagement.AppUser user, long? positionId = null)
        {
            var claims = await GenerateClaims(user, positionId);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(_siteOptions.Value.BearerTokens.AccessTokenExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_siteOptions.Value.IssuerSigningKey)), SecurityAlgorithms.HmacSha256Signature),


            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var accessToken = tokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();


            var tokenModel = new Token(user.Id, refreshToken, accessToken, DateTime.Now.AddMinutes(_siteOptions.Value.BearerTokens.RefreshTokenExpirationMinutes));

            _tokenRepository.Add(tokenModel);
            await _tokenRepository.UnitOfWork.SaveChangesAsync(new CancellationToken());

            return Tuple.Create(accessToken, refreshToken);
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_siteOptions.Value.IssuerSigningKey)),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            //if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
            if (jwtSecurityToken == null)
                throw new SecurityTokenException("توکن نامعتبر");
            return principal;
        }

        private async Task<List<Claim>> GenerateClaims(Domain.Common.Entities.UserManagement.AppUser user, long? positionId = null)
        {
            PositionDto? pos = null;
            if (positionId == null)
            {
                var poses = await _context.Set<UserPosition>().Include(c => c.Position).AsNoTracking().Where(c => c.IsActive && c.UserId == user.Id).Select(c => c.Position).ToListAsync();
                if (!poses.IsNullOrEmpty())
                {
                    var ttt = _mapper.Map<PositionDto>(poses[0]);
                    pos = poses.Select(c => _mapper.Map<PositionDto>(c)).ToList()[0];

                }
            }
            else
            {
                pos = await _positionQueryService.GetPositionById((long)positionId);
            }
            //  .Where(c => (filter.UserId != null ? c.Role.UserRoles.Any(d => d.UserId == filter.UserId) : true) &&
            //(filter.RoleId != null ? c.RoleId == filter.RoleId : true))
            var roleClaims = _context.Set<ReadModel.Entities.AppRoleClaim>().AsNoTracking().Where(c => c.Role.UserRoles.Any(d => d.UserId == user.Id)).ToList();

            var roles =_roleQueryService.FindUserRoles(user.Id);

            List<Claim> claims = new List<Claim>();
            claims.AddRange(roles.Select(c => new Claim("roles", c.Name)).ToList());

            claims.Add(new Claim("position", (string)(pos?.Title ?? string.Empty)));
            claims.Add(new Claim("positionId", pos?.Id.ToString() ?? string.Empty));
            var permissions = roleClaims.Where(c => c.ClaimType == "Permission").Select(c => new Claim("permission", c.ClaimValue)).ToList();
            if (permissions.Count == 1)
                permissions.Add(new Claim("permission", "temp"));

            claims.AddRange(permissions);

            //claims.AddRange(roleClaims.Where(c => c.ClaimType == "Permission").Select(c => new Claim("permission", c.ClaimValue)).ToList());
            claims.Add(new Claim("Id", user.Id.ToString()));
            claims.Add(new Claim("guid", user.Guid.ToString()));
            claims.Add(new Claim("FullName", user.PersianFullName));
            claims.Add(new Claim("Name", user.UserName));
            return claims;

        }
    }
}
