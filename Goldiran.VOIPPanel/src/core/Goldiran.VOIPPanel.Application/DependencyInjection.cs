using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR.Pipeline;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.Application.Common.Behaviours;
using Goldiran.VOIPPanel.Application.Common.Services.CodeProvider;
using Goldiran.VOIPPanel.Application.Common.Services.IdentityService;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using FluentValidation;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using SixLaborsCaptcha.Mvc.Core;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.IServices;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Services;
using Goldiran.VOIPPanel.Application.Common.Services.ExportToExcell.IServices;
using Goldiran.VOIPPanel.Application.Common.Services.ExportToExcell.Services;
using Saramad.Core.ApplicationService.Common.Services.ExportToExcell.Services;

namespace Goldiran.VOIPPanel.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddBusinessRules(configuration);
        //services.AddBusinessLogics(configuration);


        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        //services.AddScoped(provider => new MapperConfiguration(cfg =>
        //{
        //    cfg.AddProfile(new SuborderMapper(provider.GetService<IApplicationDbContext>()));
        //}));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        //services.AddStackExchangeRedisCache(options =>
        //{
        //    options.Configuration = "localhost";
        //    options.InstanceName = "SampleInstance";
        //});
        services.AddDistributedSqlServerCache(options =>
        {
            options.ConnectionString =
                configuration.GetConnectionString("CachConnection");
            options.SchemaName = "dbo";
            options.TableName = "CacheTable";
        });
        services.AddTransient(typeof(IRequestPreProcessor<>), typeof(LoggingBehaviour<>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandPresetBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
        services.AddMemoryCache();

        services.AddScoped<IPrincipal>(provider =>
            provider.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.User ?? ClaimsPrincipal.Current);
        services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();

        services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, ApplicationClaimsPrincipalFactory>();
        services.AddScoped<UserClaimsPrincipalFactory<AppUser, AppRole>, ApplicationClaimsPrincipalFactory>();
        //services.AddScoped<IUsedPasswordsService, UsedPasswordsService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICodeProvider, CodeProvider>();
        //services.AddScoped<IViewRendererService, ViewRendererService>();
        //services.AddScoped<IdentityErrorDescriber, CustomIdentityErrorDescriber>();


        //services.AddWebMailService();
        //services.AddScoped<IEmailSender, AuthMessageSender>();
        //services.AddScoped<ISMSSender, AuthMessageSender>();
        //services.AddScoped<IExportUtility, ExportUtility>();
        //services.AddScoped<IPackageExportUtility, PackageExportUtility>();
        //services.AddScoped<IExcelFileGenerationService, ExcelFileGenerationService>();

        services.AddAuthentication();
        services.AddScoped<ICaptchaService, CaptchaService>();
        services.AddSixLabCaptcha(x =>
        {
            x.DrawLines = 4;
        });

        services.AddScoped<IQueueStatusService, QueueStatusService>();
        services.AddScoped<IOperationRasadService, OperationRasadService>();
        services.AddScoped<IChanSpyService,ChanSpyService>();
        services.AddScoped<IQueueRasadService, QueueRasadService>();
        services.AddScoped<IChangeUserSatusService, ChangeUserSatusService>();

        //IChangeUserSatusService
        services.AddScoped<IExportUtility, ExportUtility>();
        services.AddScoped<IPackageExportUtility, PackageExportUtility>();
        services.AddScoped<IExcelFileGenerationService, ExcelFileGenerationService>();

        return services;
    }




}

