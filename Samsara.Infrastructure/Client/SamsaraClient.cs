using Samsara.Infrastructure.Requests;
using Samsara.Infrastructure.Responses;
using System.Net.Http.Json;

namespace Samsara.Infrastructure.Client;

public sealed class SamsaraClient : ISamsaraClient
{
    private readonly HttpClient _httpClient;

    public SamsaraClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// This endpoint returns a list of all sensors that are currently registered in the Samsara system.
    /// Each sensor includes details such as its ID, name, and MAC address.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<SensorResponse> GetSensorsAsync(CancellationToken ct = default)
    {
        var response = await _httpClient.GetFromJsonAsync<SensorResponse>(
            "/v1/sensors/list", ct);

        return response ?? new SensorResponse([]);
    }


    /// <summary>
    /// This endpoint returns a list of all gateways that are currently registered in the Samsara system. 
    /// Each gateway includes details such as serial number, model, connection status, and data usage.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<GatewayResponse> GetGatewaysAsync(CancellationToken ct = default)
    {
        var response = await _httpClient.GetFromJsonAsync<GatewayResponse>(
            "/gateways", ct);

        return response ?? new GatewayResponse(
           [],
           new PaginationInfo(string.Empty, false)
       );
    }

    /// <summary>
    /// This endpoint returns a list of all trailers that are currently registered in the Samsara system.
    /// Each trailer includes details such as ID, name, installed gateway, tags, and license plate information.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<TrailerResponse> GetTrailersAsync(CancellationToken ct = default)
    {
        var response = await _httpClient.GetFromJsonAsync<TrailerResponse>(
            "/fleet/trailers", ct);
        return response ?? new TrailerResponse(
            [],
            new PaginationInfo(string.Empty, false)
        );
    }

    /// <summary>
    /// This endpoint returns temperature data for the specified sensors.
    /// Requires a list of sensor IDs in the request body.
    /// </summary>
    /// <param name="sensorIds"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<SensorTemperatureResponse> GetSensorTemperaturesAsync(List<long> sensorIds, CancellationToken ct = default)
    {
        var request = new SensorTemperatureRequest(sensorIds);
        var response = await _httpClient.PostAsJsonAsync("/v1/sensors/temperature", request, ct);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<SensorTemperatureResponse>(ct);
        return result ?? new SensorTemperatureResponse(0, []);
    }
}
