using Samsara.Infrastructure.Client;

namespace Samsara.ETL.Features.SensorTemperatureSync
{
    public class SensorTemperatureSyncService
    {
        private readonly ISamsaraClient _samsaraClient;

        public SensorTemperatureSyncService(ISamsaraClient samsaraClient)
        {
            _samsaraClient = samsaraClient;
        }

        public async Task<List<SensorTemperatureDto>> SyncSensorTemperaturesAsync(List<long> sensorIds, CancellationToken ct = default)
        {
            var response = await _samsaraClient.GetSensorTemperaturesAsync(sensorIds, ct);

            var temperatureDtos = response.Sensors.Select(s => new SensorTemperatureDto(
                SensorId: s.Id,
                Name: s.Name,
                AmbientTemperature: s.AmbientTemperature,
                AmbientTemperatureTime: s.AmbientTemperatureTime,
                ProbeTemperature: s.ProbeTemperature,
                ProbeTemperatureTime: s.ProbeTemperatureTime,
                TrailerId: s.TrailerId
            )).ToList();

            //TODO: Map to database models and save to database

            return temperatureDtos;
        }
    }
}
