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
        /// This endpoint returns a list of all sensors that are currently registered in the Samsara system. Each sensor includes details such as its ID, name, and MAC address.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<SensorResponse> GetSensorsAsync(CancellationToken ct = default)
        {
            var response = await _httpClient.GetFromJsonAsync<SensorResponse>(
                "/v1/sensors/list", ct);

            return response ?? new SensorResponse(Array.Empty<Sensor>());
        }
}
