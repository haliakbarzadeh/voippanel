using Azure;
using Voip.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.MonitoredPositions.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettingSettings.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitationSettings.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Tokens.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Goldiran.VOIPPanel.Persistence;
using Goldiran.VOIPPanel.Persistence.Common.Behaviours;
using Goldiran.VOIPPanel.Persistence.Common.UserManagement.Roles;
using Goldiran.VOIPPanel.Persistence.Common.UserManagement.Users;
using Goldiran.VOIPPanel.Persistence.Files;
using Goldiran.VOIPPanel.Persistence.MonitoredPositions;
using Goldiran.VOIPPanel.Persistence.Operations;
using Goldiran.VOIPPanel.Persistence.OperationSettings;
using Goldiran.VOIPPanel.Persistence.Positions;
using Goldiran.VOIPPanel.Persistence.QueueLimitations;
using Goldiran.VOIPPanel.Persistence.Tokens;
using Goldiran.VOIPPanel.Persistence.UserPositions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Goldiran.VOIPPanel.Persistence.ContactDetails;
using Goldiran.VOIPPanel.Domain.AggregatesModel.FlatDataJobs.Contracts;
using Goldiran.VOIPPanel.Persistence.FlatDataJobs;
using Goldiran.VOIPPanel.Domain.AggregatesModel.AnsweringMachines.Contracts;
using Goldiran.VOIPPanel.Persistence.AnsweringMachines;
using MySqlConnector;
using Goldiran.VOIPPanel.Domain.AggregatesModel.TempFlatDataJobs.Contracts;
using Goldiran.VOIPPanel.Persistence.TempFlatDataJobs;
using Goldiran.VOIPPanel.Domain.AggregatesModel.SoftPhoneEvents.Contracts;
using Goldiran.VOIPPanel.Persistence.SoftPhoneEvents;


namespace Goldiran.VOIPPanel.ReadModel;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistantDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {

        //if (siteSettings == null) throw new ArgumentNullException(nameof(siteSettings));
        //services.AddEntityFrameworkSqlServer();
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<VOIPPanelContext>((iservice, options) =>
            {
                options.UseInMemoryDatabase("Saramad.CoreDb");
                options.UseInternalServiceProvider(iservice);
            }

                );
        }
        else
        {
            services.AddDbContext<VOIPPanelContext>((options) =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b =>
                    {
                        b.MigrationsAssembly(typeof(VOIPPanelContext).Assembly.FullName);
                        b.EnableRetryOnFailure();
                    });
                //.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.pe)); ;
                //options.UseInternalServiceProvider(iservice);
            }
                );

            services.AddScoped(c => new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

            //(serviceProvider, optionsBuilder) =>
            //{
            //    optionsBuilder.UseSqlServer("...");
            //    optionsBuilder.UseInternalServiceProvider(serviceProvider);
            //});
        }


        services.AddScoped<IAppUserStore, AppUserStore>();
        services.AddTransient<UserStore<AppUser, AppRole, VOIPPanelContext, long, AppUserClaim, AppUserRole, AppUserLogin, AppUserToken, AppRoleClaim>, AppUserStore>();

        services.AddScoped<IAppUserManager, AppUserManager>();
        services.AddScoped<UserManager<AppUser>, AppUserManager>();

        services.AddScoped<IAppRoleManager, AppRoleManager>();
        services.AddScoped<RoleManager<AppRole>, AppRoleManager>();

        services.AddScoped<IApplicationSignInManager, ApplicationSignInManager>();
        services.AddScoped<SignInManager<AppUser>, ApplicationSignInManager>();

        services.AddScoped<IdentityErrorDescriber, CustomIdentityErrorDescriber>();
        services.AddScoped<IAppRoleStore, AppRoleStore>();
        services.AddTransient<RoleStore<AppRole, VOIPPanelContext, long, AppUserRole, AppRoleClaim>, AppRoleStore>();
        services.AddIdentity<AppUser, AppRole>(identityOptions =>
        {
            identityOptions.Password.RequireNonAlphanumeric = false;
            identityOptions.Password.RequireDigit = false;
            identityOptions.Password.RequireLowercase = false;
            identityOptions.Password.RequireUppercase = false;
            identityOptions.Password.RequiredLength = 5;
            identityOptions.Password.RequireUppercase = false;
            //setPasswordOptions(identityOptions.Password, siteSettings);
            //setSignInOptions(identityOptions.SignIn, siteSettings);
            //setUserOptions(identityOptions.User);
            //setLockoutOptions(identityOptions.Lockout, siteSettings);
        })
    .AddUserStore<AppUserStore>()
  .AddUserManager<AppUserManager>()
  .AddRoleStore<AppRoleStore>()
  .AddRoleManager<AppRoleManager>()
  .AddSignInManager<ApplicationSignInManager>()
  .AddErrorDescriber<CustomIdentityErrorDescriber>()
  // You **cannot** use .AddEntityFrameworkStores() when you customize everything
  //.AddEntityFrameworkStores<AppDbContext, int>()
  .AddDefaultTokenProviders();
        services.AddAuthentication(option => { option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero,
                //ValidateIssuer = true,
                //ValidateAudience = true,
                //ValidateLifetime = true,
                //ValidateIssuerSigningKey = true,
                //ValidIssuer = Configuration["Jwt:Issuer"],
                //ValidAudience = Configuration["Jwt:Issuer"],
                //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(siteSettings.IssuerSigningKey)),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("IssuerSigningKey"))),

            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        //context.Properties.RedirectUri="https://saramad2.kashef.ir/api/v1/Account/refreshtoken";
                        context.Response.Headers.Add(new KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>("IsExpired", "true"));
                        //context.Response.Redirect("https://saramad2.kashef.ir/api/v1/Account/refreshtoken");
                    }
                    return Task.CompletedTask;
                }
            };
        });
        //services.ConfigureApplicationCookie(options =>
        //{
        //    options.Cookie.HttpOnly = true; // Set HTTP-only
        //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use Secure cookies (HTTPS)
        //    options.Cookie.SameSite = SameSiteMode.Strict; // Optional: Configure SameSite policy
        //});

        //Repositories
        services.AddScoped<IUnitOfWork, VOIPPanelContext>();
        services.AddScoped<IPositionRepository, PositionRepository>();
        services.AddScoped<IMonitoredPositionRepository, MonitoredPositionRepository>();
        services.AddScoped<IUserPositionRepository, UserPositionRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IOperationRepository, OperationRepository>();
        services.AddScoped<IOperationSettingRepository, OperationSettingRepository>();
        services.AddScoped<IQueueLimitationRepository, QueueLimitationRepository>();
        services.AddScoped<IQueuRepository, QueuRepository>();
        services.AddScoped<IContactDetailRepository, ContactDetailRepository>();
        services.AddScoped<IFlatDataJobRepository, FlatDataJobRepository>();
        services.AddScoped<IAnsweringMachineRepository, AnsweringMachineRepository>();
        services.AddScoped<ITempFlatDataJobRepository, TempFlatDataJobRepository>();
        services.AddScoped<ISoftPhoneEventsRepository, SoftPhoneEventsRepository>();


        //
        services.AddScoped(typeof(ITransactionService<>), typeof(TransactionService<>));
        //
        //services.AddScoped(c => new MySqlConnection(configuration.GetConnectionString("MySQLDapperConnection")));

        //
        services.AddDatabaseDeveloperPageExceptionFilter();

        return services;
    }

}
