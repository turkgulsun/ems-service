using EmsService.Application.Interfaces;
using EmsService.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace EmsService.Infrastructure.Config;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDynamoDb(configuration);
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        
        return services;
    }
}

