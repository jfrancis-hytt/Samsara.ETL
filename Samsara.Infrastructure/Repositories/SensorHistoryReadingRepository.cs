using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;

namespace Samsara.Infrastructure.Repositories;

public class SensorHistoryReadingRepository : ISensorHistoryReadingRepository
{
    private readonly string _connectionString;

    public SensorHistoryReadingRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SamsaraDbConnectionString")!;
    }

    public async Task InsertAsync(SensorHistoryReading reading)
    {
        const string sql = """
            INSERT INTO [dbo].[SensorHistoryReadings]
                (SensorId, TimeMs, ProbeTemperature, AmbientTemperature, CreatedAt)
            VALUES
                (@SensorId, @TimeMs, @ProbeTemperature, @AmbientTemperature, GETUTCDATE())
            """;

        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, reading);
    }

    public async Task InsertBatchAsync(IEnumerable<SensorHistoryReading> readings)
    {
        const string sql = """
            INSERT INTO [dbo].[SensorHistoryReadings]
                (SensorId, TimeMs, ProbeTemperature, AmbientTemperature, CreatedAt)
            VALUES
                (@SensorId, @TimeMs, @ProbeTemperature, @AmbientTemperature, GETUTCDATE())
            """;

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(sql, readings);
    }
}
