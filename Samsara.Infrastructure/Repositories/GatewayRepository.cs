using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;

namespace Samsara.Infrastructure.Repositories;

public class GatewayRepository : IGatewayRepository
{
    private readonly string _connectionString;

    public GatewayRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SamsaraDbConnectionString")!;
    }

    public async Task UpsertAsync(Gateway gateway)
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
                    UpdatedAt = GETUTCDATE()
            WHEN NOT MATCHED THEN
                INSERT (Serial, Model, AssetId, SamsaraSerial, SamsaraVin, HealthStatus,
                        LastConnected, CellularDataUsageBytes, HotspotUsageBytes, CreatedAt, UpdatedAt)
                VALUES (@Serial, @Model, @AssetId, @SamsaraSerial, @SamsaraVin, @HealthStatus,
                        @LastConnected, @CellularDataUsageBytes, @HotspotUsageBytes, GETUTCDATE(), GETUTCDATE());
            """;

        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, gateway);
    }

    public async Task<IReadOnlyList<Gateway>> GetAllAsync()
    {
        const string sql = """
            SELECT Serial, Model, AssetId, SamsaraSerial, SamsaraVin, HealthStatus,
                   LastConnected, CellularDataUsageBytes, HotspotUsageBytes, CreatedAt, UpdatedAt
            FROM [dbo].[Gateways]
            """;

        using var connection = new SqlConnection(_connectionString);
        var results = await connection.QueryAsync<Gateway>(sql);
        return results.ToList();
    }
}
