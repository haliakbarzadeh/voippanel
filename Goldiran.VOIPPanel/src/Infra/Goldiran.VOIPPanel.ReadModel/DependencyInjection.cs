using MediatR.Pipeline;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.EFCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.MonitoredPositions.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Tokens.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using Microsoft.Extensions.Options;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations.Contracts;
using Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;
using Goldiran.VOIPPanel.QueryHandler.Common;
using MySqlConnector;
using Cube.UserManagementService.Domain.AggregatesModel.UserPositions.Contracts;
using Goldiran.VOIPPanel.ReadModel.IQueries;


namespace Goldiran.VOIPPanel.QueryHandler;

public static class DependencyInjection
{
    public static IServiceCollection AddReadModelDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {

        //if (siteSettings == null) throw new ArgumentNullException(nameof(siteSettings));
        //services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //services.AddEntityFrameworkSqlServer();
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<VOIPPanelReadModelContext>((iservice, options) =>
            {
                options.UseInMemoryDatabase("Saramad.CoreDb");
                options.UseInternalServiceProvider(iservice);
            }

                );
        }
        else
        {
            services.AddDbContext<VOIPPanelReadModelContext>((options) =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("ReadModelConnection"),
                    b =>
                    {
                        b.MigrationsAssembly(typeof(VOIPPanelReadModelContext).Assembly.FullName);
                        b.EnableRetryOnFailure();
                    });
                options.EnableSensitiveDataLogging();
                //options.UseInternalServiceProvider(iservice);
            }
                );

            services.AddDbContext<AsteriskReadModelContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("MySQLEFConnection"),
                    ServerVersion.AutoDetect(configuration.GetConnectionString("MySQLEFConnection"))
                ));

            services.AddScoped(c => new SqlConnection(configuration.GetConnectionString("DapperConnection")));
            services.AddScoped(c => new MySqlConnection(configuration.GetConnectionString("MySQLDapperConnection")));
            //services.AddScoped(provider =>
            //new SqlConnectionWrapper(
            //   provider.GetRequiredService<SqlConnection>(),
            //   provider.GetRequiredService<SqlConnection>()));


            //(serviceProvider, optionsBuilder) =>
            //{
            //    optionsBuilder.UseSqlServer("...");
            //    optionsBuilder.UseInternalServiceProvider(serviceProvider);
            //});
        }
        services.AddScoped<IReadModelContext, VOIPPanelReadModelContext>();
        //Query Services
        services.AddScoped<IUserQueryService, UserQueryService>();
        services.AddScoped<IRoleQueryService, RoleQueryService>();
        services.AddScoped<IMonitoredPositionQueryService, MonitoredPositionQueryService>();
        services.AddScoped<IPositionQueryService, PositionQueryService>();
        services.AddScoped<IUserPositionQueryService, UserPositionQueryService>();
        services.AddScoped<IFileQueryService, FileQueryService>();
        services.AddScoped<IQueuQueryService, QueuQueryService>();
        services.AddScoped<IOperationQueryService, OperationQueryService>();
        services.AddScoped<IOperationSettingQueryService, OperationSettingQueryService>();
        services.AddScoped<IQueueLimitationQueryService, QueueLimitationQueryService>();
        services.AddScoped<IRasadOperationQueryService, RasadOperationQueryService>();
        services.AddScoped<IContactDetailQueryService, ContactDetailsQueryService>();
        services.AddScoped<IFlatDataJobQueryService, FlatDataJobQueryService>();
        services.AddScoped<IAskCustomerQueryService, AskCustomerQueryService>();
        services.AddScoped<ISecureCallQueryService, SecureCallQueryService>();
        services.AddScoped<IWallboardReportQueryService, WallboardReportQueryService>();
        services.AddScoped<IMasterWallboardReportQueryService, MasterWallboardReportQueryService>();
        services.AddScoped<IQueueLogQueryService, QueueLogQueryService>();
        services.AddScoped<IAnsweringMachineQueryService, AnsweringMachineQueryService>();
        services.AddScoped<ITempFlatDataJobQueryService, TempFlatDataJobQueryService>();
        services.AddScoped<IQueueLogDetailsQueryService, QueueLogDetailsQueryService>();
        services.AddScoped<IRemainedCallQueryService, RemainedCallQueryService>();
        services.AddScoped<ISoftPhoneEventQueryService, SoftPhoneEventQueryService>();
        services.AddScoped<IServiceWallboardReportQueryService, ServiceWallboardReportQueryService>();

        //

        return services;
    }

}
