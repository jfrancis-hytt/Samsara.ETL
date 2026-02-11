using Microsoft.Extensions.Logging;
using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;
using Samsara.Infrastructure.Dtos;
using Samsara.Infrastructure.Samsara.Client;

namespace Samsara.Infrastructure.Services;

public class SensorTemperatureService
{
    private readonly ISamsaraClient _samsaraClient;
    private readonly ISensorRepository _sensorRepository;
    private readonly ISensorTemperatureReadingRepository _sensorTemperatureReadingRepository;
    private readonly ILogger<SensorTemperatureService> _logger;

    public SensorTemperatureService(
        ISamsaraClient samsaraClient,
        ISensorRepository sensorRepository,
        ISensorTemperatureReadingRepository sensorTemperatureReadingRepository,
        ILogger<SensorTemperatureService> logger)
    {
        _samsaraClient = samsaraClient;
        _sensorRepository = sensorRepository;
        _sensorTemperatureReadingRepository = sensorTemperatureReadingRepository;
        _logger = logger;
    }

    public async Task<List<SensorTemperatureDto>> SyncSensorTemperaturesAsync(CancellationToken ct = default)
    {
        var sensors = await _sensorRepository.GetAllAsync();
        var sensorIds = sensors.Select(s => s.SensorId).ToList();

        if (sensorIds.Count == 0)
        {
            _logger.LogWarning("No sensors found in database — skipping temperature sync");
            return [];
        }

        var response = await _samsaraClient.GetSensorTemperaturesAsync(sensorIds, ct);

        var temperatureDtos = response.Sensors.Select(s => new SensorTemperatureDto(
            SensorId: s.Id ?? 0,
            Name: s.Name ?? string.Empty,
            AmbientTemperature: s.AmbientTemperature,
            AmbientTemperatureTime: s.AmbientTemperatureTime,
            ProbeTemperature: s.ProbeTemperature,
            ProbeTemperatureTime: s.ProbeTemperatureTime,
            TrailerId: s.TrailerId
        )).ToList();

        var entities = temperatureDtos.Select(dto => new SensorTemperatureReadingEntity
        {
            SensorId = dto.SensorId,
            Name = dto.Name,
            AmbientTemperature = dto.AmbientTemperature,
            AmbientTemperatureTime = dto.AmbientTemperatureTime,
            ProbeTemperature = dto.ProbeTemperature,
            ProbeTemperatureTime = dto.ProbeTemperatureTime,
            TrailerId = dto.TrailerId
        });
        await _sensorTemperatureReadingRepository.InsertBatchAsync(entities);

        return temperatureDtos;
    }
}
