using Samsara.Infrastructure.Client;
using Samsara.Infrastructure.Responses;

namespace Samsara.ETL.Features.TrailerSync;

public class TrailerSyncService
{
    private readonly ISamsaraClient _samsaraClient;

    public TrailerSyncService(ISamsaraClient samsaraClient)
    {
        _samsaraClient = samsaraClient;
    }

    public async Task<List<TrailerDto>> SyncTrailersAsync(CancellationToken ct = default)
    {
        var response = await _samsaraClient.GetTrailersAsync(ct);

        var trailerDtos = response.Data.Select(t => new TrailerDto(
            Id: t.Id,
            Name: t.Name,
            GatewaySerial: t.InstalledGateway?.Serial,
            GatewayModel: t.InstalledGateway?.Model,
            SamsaraSerial: t.ExternalIds?.SamsaraSerial,
            SamsaraVin: t.ExternalIds?.SamsaraVin,
            LicensePlate: t.LicensePlate,
            Notes: t.Notes,
            EnabledForMobile: t.EnabledForMobile,
            Tags: t.Tags?.Select(tag => new TrailerTagDto(
                Id: tag.Id,
                Name: tag.Name,
                ParentTagId: tag.ParentTagId
            )).ToList()
        )).ToList();

        //TODO: Map to database models and save to database

        return trailerDtos;
    }
}