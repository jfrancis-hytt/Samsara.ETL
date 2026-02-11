using Dapper;
using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;

namespace Samsara.Infrastructure.Repositories;

public class SensorTemperatureReadingRepository : ISensorTemperatureReadingRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SensorTemperatureReadingRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InsertBatchAsync(IEnumerable<SensorTemperatureReadingEntity> readings)
    {
        // DateTimeKeys are using 112 style here is a link for reference: https://learn.microsoft.com/en-us/sql/t-sql/functions/cast-and-convert-transact-sql?view=sql-server-ver17
        const string sql = """
            INSERT INTO dbo.SensorTemperatureReadings
                (SensorId, Name, AmbientTemperature, AmbientTemperatureTime,
                 ProbeTemperature, ProbeTemperatureTime, TrailerId, CreatedAt)
            SELECT
                @SensorId, @Name, @AmbientTemperature, @AmbientTemperatureTime,
                @ProbeTemperature, @ProbeTemperatureTime, @TrailerId, SYSUTCDATETIME()
            WHERE NOT EXISTS (
                SELECT 1
                FROM dbo.SensorTemperatureReadings t
                WHERE t.SensorId = @SensorId
                  AND t.AmbientTimeKey = ISNULL(@AmbientTemperatureTime, CONVERT(datetime2(0), '19000101', 112))
                  AND t.ProbeTimeKey   = ISNULL(@ProbeTemperatureTime,   CONVERT(datetime2(0), '19000101', 112))
                  AND t.TrailerIdKey   = ISNULL(@TrailerId, -1)
            );
            """;

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        await connection.ExecuteAsync(sql, readings);
    }
}
