using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;

namespace Samsara.Infrastructure.Repositories;

public class SensorTemperatureReadingRepository : ISensorTemperatureReadingRepository
{
    private readonly string _connectionString;

    public SensorTemperatureReadingRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SamsaraDbConnectionString")!;
    }

    public async Task InsertAsync(SensorTemperatureReading reading)
    {
        const string sql = """
            INSERT INTO [dbo].[SensorTemperatureReadings]
                (SensorId, Name, AmbientTemperature, AmbientTemperatureTime,
                 ProbeTemperature, ProbeTemperatureTime, TrailerId, CreatedAt)
            VALUES
                (@SensorId, @Name, @AmbientTemperature, @AmbientTemperatureTime,
                 @ProbeTemperature, @ProbeTemperatureTime, @TrailerId, GETUTCDATE())
            """;

        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, reading);
    }
}
