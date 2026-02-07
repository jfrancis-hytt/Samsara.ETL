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

        return response ?? new SensorResponse(Array.Empty<Sensor>());
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
           new List<Gateway>(),
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
            new List<Trailer>(),
            new PaginationInfo(string.Empty, false)
        );
    }
}
