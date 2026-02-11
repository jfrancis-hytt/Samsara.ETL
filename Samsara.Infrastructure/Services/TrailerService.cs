using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;
using Samsara.Infrastructure.Dtos;
using Samsara.Infrastructure.Samsara.Client;

namespace Samsara.Infrastructure.Services;

public class TrailerService
{
    private readonly ISamsaraClient _samsaraClient;
    private readonly ITrailerRepository _trailerRepository;
    private readonly ITrailerTagRepository _trailerTagRepository;

    public TrailerService(
        ISamsaraClient samsaraClient,
        ITrailerRepository trailerRepository,
        ITrailerTagRepository trailerTagRepository)
    {
        _samsaraClient = samsaraClient;
        _trailerRepository = trailerRepository;
        _trailerTagRepository = trailerTagRepository;
    }

    public async Task<List<TrailerDto>> SyncTrailersAsync(CancellationToken ct = default)
    {
        var response = await _samsaraClient.GetTrailersAsync(ct);

        var trailerDtos = response.Data.Select(t => new TrailerDto(
            Id: t.Id,
            Name: t.Name ?? string.Empty,
            GatewaySerial: t.InstalledGateway?.Serial,
            GatewayModel: t.InstalledGateway?.Model,
            SamsaraSerial: t.ExternalIds?.SamsaraSerial,
            SamsaraVin: t.ExternalIds?.SamsaraVin,
            LicensePlate: t.LicensePlate,
            Notes: t.Notes,
            EnabledForMobile: t.EnabledForMobile ?? false,
            Tags: t.Tags?.Select(tag => new TrailerTagDto(
                Id: tag.Id,
                Name: tag.Name,
                ParentTagId: tag.ParentTagId ?? string.Empty
            )).ToList()
        )).ToList();

        var entities = trailerDtos.Select(dto => new TrailerEntity
        {
            Id = dto.Id,
            Name = dto.Name,
            GatewaySerial = dto.GatewaySerial,
            GatewayModel = dto.GatewayModel,
            SamsaraSerial = dto.SamsaraSerial,
            SamsaraVin = dto.SamsaraVin,
            LicensePlate = dto.LicensePlate,
            Notes = dto.Notes,
            EnabledForMobile = dto.EnabledForMobile
        });
        await _trailerRepository.UpsertBatchAsync(entities);

        var allTags = trailerDtos.SelectMany(dto =>
            dto.Tags?.Select(t => new TrailerTagEntity
            {
                Id = t.Id,
                Name = t.Name,
                ParentTagId = t.ParentTagId,
                TrailerId = dto.Id
            }) ?? []);
        await _trailerTagRepository.ReplaceAllAsync(allTags);

        return trailerDtos;
    }
}
