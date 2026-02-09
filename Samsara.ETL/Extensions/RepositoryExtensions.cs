using Microsoft.Extensions.DependencyInjection;
using Samsara.Domain.Repositories;
using Samsara.Infrastructure.Repositories;

namespace Samsara.ETL.Extensions;

public static class RepositoryExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISensorRepository, SensorRepository>();
        services.AddScoped<IGatewayRepository, GatewayRepository>();
        services.AddScoped<IAccessoryDeviceRepository, AccessoryDeviceRepository>();
        services.AddScoped<ITrailerRepository, TrailerRepository>();
        services.AddScoped<ITrailerTagRepository, TrailerTagRepository>();
        services.AddScoped<ISensorTemperatureReadingRepository, SensorTemperatureReadingRepository>();
    }
}
