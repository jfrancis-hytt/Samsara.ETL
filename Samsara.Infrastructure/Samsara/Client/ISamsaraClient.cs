using Samsara.Infrastructure.Samsara.Requests;
using Samsara.Infrastructure.Samsara.Responses;

namespace Samsara.Infrastructure.Samsara.Client;

public interface ISamsaraClient
{
    Task<SensorResponse> GetSensorsAsync(CancellationToken ct = default);
    Task<GatewayResponse> GetGatewaysAsync(CancellationToken ct = default);
    Task<TrailerResponse> GetTrailersAsync(CancellationToken ct = default);
    Task<SensorTemperatureResponse> GetSensorTemperaturesAsync(List<long> sensorIds, CancellationToken ct = default);
    Task<SensorHistoryResponse> GetSensorHistoryAsync(SensorHistoryRequest request, CancellationToken ct = default);

}
