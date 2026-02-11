using Dapper;
using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;

namespace Samsara.Infrastructure.Repositories;

public class SensorHistoryReadingRepository : ISensorHistoryReadingRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SensorHistoryReadingRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InsertBatchAsync(IEnumerable<SensorHistoryReadingEntity> readings)
    {
        const string sql = """
            INSERT INTO [dbo].[SensorHistoryReadings]
                (SensorId, TimeMs, ProbeTemperature, AmbientTemperature, CreatedAt)
            SELECT
                @SensorId, @TimeMs, @ProbeTemperature, @AmbientTemperature, SYSUTCDATETIME()
            WHERE NOT EXISTS (
                SELECT 1 FROM [dbo].[SensorHistoryReadings]
                WHERE SensorId = @SensorId
                  AND TimeMs = @TimeMs
            )
            """;

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        await connection.ExecuteAsync(sql, readings);
    }
}
