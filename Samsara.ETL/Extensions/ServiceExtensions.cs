using Microsoft.Extensions.DependencyInjection;
using Samsara.ETL.Features.SensorSync;

namespace Samsara.ETL.Extensions;

public static class ServiceExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        // Register DI Services
        services.AddScoped<SensorSyncService>();
        services.AddScoped<SensorSyncJob>();

    }
}
