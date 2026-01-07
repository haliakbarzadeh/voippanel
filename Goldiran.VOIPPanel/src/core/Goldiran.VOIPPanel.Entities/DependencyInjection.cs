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



namespace Goldiran.VOIPPanel.ReadModel;

public static class DependencyInjection
{
    public static IServiceCollection AddReadModelEntitiesDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }

}
