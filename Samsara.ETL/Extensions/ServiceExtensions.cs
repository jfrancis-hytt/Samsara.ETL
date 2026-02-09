using Microsoft.Extensions.DependencyInjection;
using Samsara.ETL.Pipelines.Gateway;
using Samsara.ETL.Pipelines.Sensor;
using Samsara.ETL.Pipelines.SensorTemperature;
using Samsara.ETL.Pipelines.Trailer;
using Samsara.Infrastructure.Services;

namespace Samsara.ETL.Extensions;

public static class ServiceExtensions
{
    public static void AddServices(this IServiceCollection services)
    {

        // Sensors
        services.AddScoped<SensorService>();
        services.AddScoped<SensorJob>();

        // Sensor Temps
        services.AddScoped<SensorTemperatureService>();
        services.AddScoped<SensorTemperatureSyncJob>();

        // Gateways
        services.AddScoped<GatewayService>();
        services.AddScoped<GatewayJob>();

        // Trailers
        services.AddScoped<TrailerService>();
        services.AddScoped<TrailerJob>();

    }
}