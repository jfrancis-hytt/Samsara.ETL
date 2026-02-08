using Samsara.Infrastructure.Responses;

namespace Samsara.Infrastructure.Client;

public interface ISamsaraClient
{
    Task<SensorResponse> GetSensorsAsync(CancellationToken ct = default);
    Task<GatewayResponse> GetGatewaysAsync(CancellationToken ct = default);
    Task<TrailerResponse> GetTrailersAsync(CancellationToken ct = default);
    Task<SensorTemperatureResponse> GetSensorTemperaturesAsync(List<long> sensorIds, CancellationToken ct = default);
}
