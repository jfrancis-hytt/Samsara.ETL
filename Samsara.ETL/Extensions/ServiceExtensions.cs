using Microsoft.Extensions.DependencyInjection;
using Samsara.ETL.Features.GatewaySync;
using Samsara.ETL.Features.SensorSync;
using Samsara.ETL.Features.SensorTemperatureSync;
using Samsara.ETL.Features.TrailerSync;

namespace Samsara.ETL.Extensions;

public static class ServiceExtensions
{
    public static void AddServices(this IServiceCollection services)
    {

        // Sensors
        services.AddScoped<SensorSyncService>();
        services.AddScoped<SensorSyncJob>();

        // Sensor Temps
        services.AddScoped<SensorTemperatureSyncService>();
        services.AddScoped<SensorTemperatureSyncJob>();

        // Gateways
        services.AddScoped<GatewaySyncService>();
        services.AddScoped<GatewaySyncJob>();

        // Trailers
        services.AddScoped<TrailerSyncService>();
        services.AddScoped<TrailerSyncJob>();

    }
}