using Dapper;
using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;

namespace Samsara.Infrastructure.Repositories;

public class TrailerRepository : ITrailerRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TrailerRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task UpsertBatchAsync(IEnumerable<TrailerEntity> trailers)
    {
        const string sql = """
            MERGE INTO [dbo].[Trailers] AS target
            USING (SELECT @Id AS Id) AS source
            ON target.Id = source.Id
            WHEN MATCHED THEN
                UPDATE SET
                    Name = @Name,
                    GatewaySerial = @GatewaySerial,
                    GatewayModel = @GatewayModel,
                    SamsaraSerial = @SamsaraSerial,
                    SamsaraVin = @SamsaraVin,
                    LicensePlate = @LicensePlate,
                    Notes = @Notes,
                    EnabledForMobile = @EnabledForMobile,
                    UpdatedAt = SYSUTCDATETIME()
            WHEN NOT MATCHED THEN
                INSERT (Id, Name, GatewaySerial, GatewayModel, SamsaraSerial, SamsaraVin,
                        LicensePlate, Notes, EnabledForMobile, CreatedAt, UpdatedAt)
                VALUES (@Id, @Name, @GatewaySerial, @GatewayModel, @SamsaraSerial, @SamsaraVin,
                        @LicensePlate, @Notes, @EnabledForMobile, SYSUTCDATETIME(), SYSUTCDATETIME());
            """;

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        await connection.ExecuteAsync(sql, trailers);
    }

    public async Task<IReadOnlyList<TrailerEntity>> GetAllAsync()
    {
        const string sql = """
            SELECT Id, Name, GatewaySerial, GatewayModel, SamsaraSerial, SamsaraVin,
                   LicensePlate, Notes, EnabledForMobile, CreatedAt, UpdatedAt
            FROM [dbo].[Trailers]
            """;

        using var connection = _connectionFactory.CreateConnection();
        var results = await connection.QueryAsync<TrailerEntity>(sql);
        return results.ToList();
    }
}
