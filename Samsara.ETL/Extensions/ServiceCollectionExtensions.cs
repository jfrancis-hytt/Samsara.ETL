using Microsoft.Extensions.DependencyInjection;
using Samsara.Infrastructure.Services;

namespace Samsara.ETL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<SensorService>();
        services.AddScoped<SensorHistoryService>();
        services.AddScoped<SensorTemperatureService>();
        services.AddScoped<GatewayService>();
        services.AddScoped<TrailerService>();
    }
}
