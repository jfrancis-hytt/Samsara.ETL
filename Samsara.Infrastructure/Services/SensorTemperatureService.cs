using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;
using Samsara.Infrastructure.Dtos;
using Samsara.Infrastructure.Samsara.Client;

namespace Samsara.Infrastructure.Services;

public class SensorTemperatureService
{
    private readonly ISamsaraClient _samsaraClient;
    private readonly ISensorTemperatureReadingRepository _sensorTemperatureReadingRepository;

    public SensorTemperatureService(
        ISamsaraClient samsaraClient,
        ISensorTemperatureReadingRepository sensorTemperatureReadingRepository)
    {
        _samsaraClient = samsaraClient;
        _sensorTemperatureReadingRepository = sensorTemperatureReadingRepository;
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

        foreach (var dto in temperatureDtos)
        {
            var entity = new SensorTemperatureReading
            {
                SensorId = dto.SensorId,
                Name = dto.Name,
                AmbientTemperature = dto.AmbientTemperature,
                AmbientTemperatureTime = dto.AmbientTemperatureTime,
                ProbeTemperature = dto.ProbeTemperature,
                ProbeTemperatureTime = dto.ProbeTemperatureTime,
                TrailerId = dto.TrailerId
            };
            await _sensorTemperatureReadingRepository.InsertAsync(entity);
        }

        return temperatureDtos;
    }
}
