using Dapper;
using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;

namespace Samsara.Infrastructure.Repositories;

public class SensorRepository : ISensorRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SensorRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }


    public async Task UpsertBatchAsync(IEnumerable<SensorEntity> sensors)
    {
        const string sql = """
            MERGE INTO [dbo].[Sensors] AS target
            USING (SELECT @SensorId AS SensorId) AS source
            ON target.SensorId = source.SensorId
            WHEN MATCHED THEN
                UPDATE SET
                    Name = @Name,
                    MacAddress = @MacAddress,
                    UpdatedAt = SYSUTCDATETIME()
            WHEN NOT MATCHED THEN
                INSERT (SensorId, Name, MacAddress, CreatedAt, UpdatedAt)
                VALUES (@SensorId, @Name, @MacAddress, SYSUTCDATETIME(), SYSUTCDATETIME());
            """;

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        await connection.ExecuteAsync(sql, sensors);
    }

    public async Task<IReadOnlyList<SensorEntity>> GetAllAsync()
    {
        const string sql = "SELECT SensorId, Name, MacAddress, CreatedAt, UpdatedAt FROM [dbo].[Sensors]";

        using var connection = _connectionFactory.CreateConnection();
        var results = await connection.QueryAsync<SensorEntity>(sql);
        return results.ToList();
    }
}
