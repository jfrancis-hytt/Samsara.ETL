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
        using var response = await _httpClient.GetAsync("/v1/sensors/list",ct);

        await EnsureSuccess(response, "Sensors", ct);

        var result = await response.Content.ReadFromJsonAsync<SensorResponse>(ct);
        return result ?? new SensorResponse([]);
    }


    /// <summary>
    /// This endpoint returns a list of all gateways that are currently registered in the Samsara system. 
    /// Each gateway includes details such as serial number, model, connection status, and data usage.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns><see cref="GatewayResponse"/></returns>
    public async Task<GatewayResponse> GetGatewaysAsync(CancellationToken ct = default)
    {
        var data = await PaginateAsync<GatewayResponse, Gateway>(
            "/gateways", r => r.Data, r => r.Pagination, "Gateways", ct);
        return new GatewayResponse(data, new PaginationInfo(string.Empty, false));
    }


    /// <summary>
    /// This endpoint returns a list of all trailers that are currently registered in the Samsara system.
    /// Each trailer includes details such as ID, name, installed gateway, tags, and license plate information.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns><see cref="TrailerResponse"/></returns>
    public async Task<TrailerResponse> GetTrailersAsync(CancellationToken ct = default)
    {
        var data = await PaginateAsync<TrailerResponse, Trailer>(
            "/fleet/trailers", r => r.Data, r => r.Pagination, "Trailers", ct);
        return new TrailerResponse(data, new PaginationInfo(string.Empty, false));
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
        using var response = await _httpClient.PostAsJsonAsync("/v1/sensors/temperature", request, ct);

        await EnsureSuccess(response, "Sensor temperature", ct);

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
        using var response = await _httpClient.PostAsJsonAsync("/v1/sensors/history", request, ct);

        await EnsureSuccess(response, "Sensor history", ct);

        var result = await response.Content.ReadFromJsonAsync<SensorHistoryResponse>(ct);
        return result ?? new SensorHistoryResponse([]);
    }


    private async Task<List<TItem>> PaginateAsync<TResponse, TItem>(
        string baseUrl,
        Func<TResponse, IReadOnlyList<TItem>> getData,
        Func<TResponse, PaginationInfo> getPagination,
        string errorLabel,
        CancellationToken ct)
    {
        var all = new List<TItem>();
        string? cursor = null;

        do
        {
            var url = cursor is null ? baseUrl : $"{baseUrl}?after={cursor}";
            using var response = await _httpClient.GetAsync(url, ct);
            await EnsureSuccess(response, errorLabel, ct);

            var result = await response.Content.ReadFromJsonAsync<TResponse>(ct);
            if (result is null) break;

            all.AddRange(getData(result));
            var pagination = getPagination(result);
            cursor = pagination.HasNextPage ? pagination.EndCursor : null;
        } while (cursor != null);

        return all;
    }


    private static async Task EnsureSuccess(HttpResponseMessage response, string label, CancellationToken ct)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException(
                $"{label} request failed with {(int)response.StatusCode} {response.StatusCode}: {errorBody}");
        }
    }
}
