using Samsara.Infrastructure.Client;

namespace Samsara.ETL.Features.SensorSync;

public class SensorSyncService
{
    private readonly ISamsaraClient _samsaraClient;

    public SensorSyncService(ISamsaraClient samsaraClient)
    {
        _samsaraClient = samsaraClient;
    }

    public async Task<List<SensorDto>> SyncSensorsAsync(CancellationToken ct = default)
    {
        var response = await _samsaraClient.GetSensorsAsync(ct);

        var sensorDtos = response.Sensors.Select(s => new SensorDto(
            SensorId: s.Id,
            Name: s.Name,
            MacAddress: s.MacAddress
        )).ToList();

        //TODO: Map to database models and save to database

        return sensorDtos;
    }

    
}