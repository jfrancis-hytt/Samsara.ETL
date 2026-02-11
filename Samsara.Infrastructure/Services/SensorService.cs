using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;
using Samsara.Infrastructure.Dtos;
using Samsara.Infrastructure.Samsara.Client;

namespace Samsara.Infrastructure.Services;

public class SensorService
{
    private readonly ISamsaraClient _samsaraClient;
    private readonly ISensorRepository _sensorRepository;

    public SensorService(ISamsaraClient samsaraClient, ISensorRepository sensorRepository)
    {
        _samsaraClient = samsaraClient;
        _sensorRepository = sensorRepository;
    }

    public async Task<List<SensorDto>> SyncSensorsAsync(CancellationToken ct = default)
    {
        var response = await _samsaraClient.GetSensorsAsync(ct);

        var sensorDtos = response.Sensors.Select(s => new SensorDto(
            SensorId: s.Id,
            Name: s.Name ?? string.Empty,
            MacAddress: s.MacAddress ?? string.Empty
        )).ToList();

        foreach (var dto in sensorDtos)
        {
            var entity = new Sensor
            {
                SensorId = dto.SensorId,
                Name = dto.Name,
                MacAddress = dto.MacAddress
            };
            await _sensorRepository.UpsertAsync(entity);
        }

        return sensorDtos;
    }
}
