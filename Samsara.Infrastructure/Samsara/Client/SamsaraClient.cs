using Samsara.Infrastructure.Samsara.Requests;
using Samsara.Infrastructure.Samsara.Responses;
using System.Net.Http.Json;

namespace Samsara.Infrastructure.Samsara.Client;

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
    /// <returns><see cref="GatewayResponse"/></returns>
    public async Task<GatewayResponse> GetGatewaysAsync(CancellationToken ct = default)
    {
        var allGatewayData = new List<Gateway>(); // The entire set of data from all pagination requests
        string? hasMorePages = null; // Used to see if there are more pages in the request

        do
        {
            var url = hasMorePages is null ? "/gateways" : $"/gateways?after={hasMorePages}";
            var response = await _httpClient.GetFromJsonAsync<GatewayResponse>(url, ct);

            if (response is null) // Break out because there are no more pages
                break;

            allGatewayData.AddRange(response.Data); // Add every gateway object

            hasMorePages = response.Pagination.HasNextPage ? response.Pagination.EndCursor : null; // Check if there are more pages and assign for next request. 

        } while (hasMorePages != null);


        return new GatewayResponse(allGatewayData, new PaginationInfo(string.Empty, false));
    }

    /// <summary>
    /// This endpoint returns a list of all trailers that are currently registered in the Samsara system.
    /// Each trailer includes details such as ID, name, installed gateway, tags, and license plate information.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns><see cref="TrailerResponse"/></returns>
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
    /// <returns><see cref="SensorTemperatureResponse"/></returns>
    public async Task<SensorTemperatureResponse> GetSensorTemperaturesAsync(List<long> sensorIds, CancellationToken ct = default)
    {
        var request = new SensorTemperatureRequest(sensorIds);
        var response = await _httpClient.PostAsJsonAsync("/v1/sensors/temperature", request, ct);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<SensorTemperatureResponse>(ct);
        return result ?? new SensorTemperatureResponse(0, []);
    }

    /// <summary>
    /// This endpoint returns a list of Sensor History data for the specified sensors.
    /// Requires a SensorHistoryRequest in body
    /// </summary>
    /// <param name="request"> <see cref="SensorHistoryRequest"/></param>
    /// <param name="ct"></param>
    /// <returns><see cref="SensorHistoryResponse"/></returns>
    public async Task<SensorHistoryResponse> GetSensorHistoryAsync(SensorHistoryRequest request, CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsJsonAsync("/v1/sensors/history", request, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException(
                $"Sensor history request failed with {(int)response.StatusCode} {response.StatusCode}: {errorBody}");
        }

        var result = await response.Content.ReadFromJsonAsync<SensorHistoryResponse>(ct);
        return result ?? new SensorHistoryResponse([]);
    }
}
