using Dapper;
using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;

namespace Samsara.Infrastructure.Repositories;

public class GatewayRepository : IGatewayRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GatewayRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task UpsertBatchAsync(IEnumerable<GatewayEntity> gateways)
    {
        const string sql = """
            MERGE INTO [dbo].[Gateways] AS target
            USING (SELECT @Serial AS Serial) AS source
            ON target.Serial = source.Serial
            WHEN MATCHED THEN
                UPDATE SET
                    Model = @Model,
                    AssetId = @AssetId,
                    SamsaraSerial = @SamsaraSerial,
                    SamsaraVin = @SamsaraVin,
                    HealthStatus = @HealthStatus,
                    LastConnected = @LastConnected,
                    CellularDataUsageBytes = @CellularDataUsageBytes,
                    HotspotUsageBytes = @HotspotUsageBytes,
                    UpdatedAt = SYSUTCDATETIME()
            WHEN NOT MATCHED THEN
                INSERT (Serial, Model, AssetId, SamsaraSerial, SamsaraVin, HealthStatus,
                        LastConnected, CellularDataUsageBytes, HotspotUsageBytes, CreatedAt, UpdatedAt)
                VALUES (@Serial, @Model, @AssetId, @SamsaraSerial, @SamsaraVin, @HealthStatus,
                        @LastConnected, @CellularDataUsageBytes, @HotspotUsageBytes, SYSUTCDATETIME(), SYSUTCDATETIME());
            """;

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        await connection.ExecuteAsync(sql, gateways);
    }

    public async Task<IReadOnlyList<GatewayEntity>> GetAllAsync()
    {
        const string sql = """
            SELECT Serial, Model, AssetId, SamsaraSerial, SamsaraVin, HealthStatus,
                   LastConnected, CellularDataUsageBytes, HotspotUsageBytes, CreatedAt, UpdatedAt
            FROM [dbo].[Gateways]
            """;

        using var connection = _connectionFactory.CreateConnection();
        var results = await connection.QueryAsync<GatewayEntity>(sql);
        return results.ToList();
    }
}
