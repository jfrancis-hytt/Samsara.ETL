using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Samsara.Domain.Entities;
using Samsara.Domain.Repositories;

namespace Samsara.Infrastructure.Repositories;

public class SensorRepository : ISensorRepository
{
    private readonly string _connectionString;

    public SensorRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SamsaraDbConnectionString")!;
    }

    public async Task UpsertAsync(Sensor sensor)
    {
        const string sql = """
            MERGE INTO [dbo].[Sensors] AS target
            USING (SELECT @SensorId AS SensorId) AS source
            ON target.SensorId = source.SensorId
            WHEN MATCHED THEN
                UPDATE SET
                    Name = @Name,
                    MacAddress = @MacAddress,
                    UpdatedAt = GETUTCDATE()
            WHEN NOT MATCHED THEN
                INSERT (SensorId, Name, MacAddress, CreatedAt, UpdatedAt)
                VALUES (@SensorId, @Name, @MacAddress, GETUTCDATE(), GETUTCDATE());
            """;

        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, sensor);
    }

    public async Task<IReadOnlyList<Sensor>> GetAllAsync()
    {
        const string sql = "SELECT SensorId, Name, MacAddress, CreatedAt, UpdatedAt FROM [dbo].[Sensors]";

        using var connection = new SqlConnection(_connectionString);
        var results = await connection.QueryAsync<Sensor>(sql);
        return results.ToList();
    }
}
