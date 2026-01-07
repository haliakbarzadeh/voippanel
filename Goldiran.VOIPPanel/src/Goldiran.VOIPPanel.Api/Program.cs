using FluentValidation.AspNetCore;
using Voip.Framework.Domain.CommonServices.IServices;
using Voip.Framework.Domain.CommonServices.Services;
using Goldiran.VOIPPanel.Application;
using Goldiran.VOIPPanel.QueryHandler;
using Goldiran.VOIPPanel.ReadModel;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SixLabors.ImageSharp;
using System.Reflection;
using Serilog;
using Serilog.Settings.Configuration;
using Microsoft.AspNetCore.Hosting;
using Serilog.Extensions;
using Voip.Framework.Common.AppSettings;
using Voip.Framework.Common.ActionFilters;


namespace Goldiran.VOIPPanel.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            var configuration = GetConfiguration();

            builder.Services.Configure<SiteSettings>(options => configuration.Bind(options));

            Log.Logger = CreateSerilogLogger(configuration);
            builder.Host.UseSerilog();
            //var siteSettings = builder.Services.GetSiteSettings();
            // Add services to the container.
            builder.Services.AddApplication(configuration);
            builder.Services.AddReadModelEntitiesDependencyInjection(configuration);
            builder.Services.AddReadModelDependencyInjection(configuration);
            builder.Services.AddPersistantDependencyInjection(configuration);
            builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
            //builder.Services.AddCustomIntegrations(configuration).AddEventBus(configuration);


            builder.Services.AddControllers(options =>
            {

                options.Filters.Add<ApiExceptionFilterAttribute>();

            });

            //builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            //builder.Services.AddFluentValidationClientsideAdapters();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Saramad API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                  });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            try
            {
                //var eventBus = builder.Services.BuildServiceProvider().GetRequiredService<IEventBus>();

                //eventBus.Subscribe<ProductPriceChangedIntegrationEvent, IIntegrationEventHandler<ProductPriceChangedIntegrationEvent>>();
            }
            catch (Exception)
            {


            }

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    name: "AllowAll",
                    policy =>
                    {
                        //policy.WithOrigins(IConfiguration.GetSection("allowableOrigins").Get<string[]>());

                        policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });


            var app = builder.Build();



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();

            app.MapControllers();
            app.UseSwagger(options =>
            {
                options.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Saramad API V1");
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller}/{action=Index}/{id?}");
            //    endpoints.MapRazorPages();
            //});

            app.Run();
        }

        static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            //builder.AddUserSecrets("Saramad-efad71c6-743c-4b87-9de8-f26d77146f6d");

            return builder.Build();
        }

        static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                //.WriteTo.Elasticsearch()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
        //static SiteSettings GetSiteSettings(this IServiceCollection services)
        //{
        //    var provider = services.BuildServiceProvider();
        //    var siteSettingsOptions = provider.GetRequiredService<IOptionsSnapshot<SiteSettings>>();
        //    var siteSettings = siteSettingsOptions.Value;
        //    if (siteSettings == null) throw new ArgumentNullException(nameof(siteSettings));
        //    return siteSettings;
        //}
    }
}