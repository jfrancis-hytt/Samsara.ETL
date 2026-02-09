using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;

namespace Samsara.Infrastructure.Repositories;

public class AccessoryDeviceRepository : IAccessoryDeviceRepository
{
    private readonly string _connectionString;

    public AccessoryDeviceRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SamsaraDbConnectionString")!;
    }

    public async Task ReplaceByGatewayAsync(string gatewaySerial, IEnumerable<AccessoryDevice> devices)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        using var transaction = connection.BeginTransaction();

        const string deleteSql = "DELETE FROM [dbo].[AccessoryDevices] WHERE GatewaySerial = @GatewaySerial";
        await connection.ExecuteAsync(deleteSql, new { GatewaySerial = gatewaySerial }, transaction);

        const string insertSql = """
            INSERT INTO [dbo].[AccessoryDevices] (Serial, Model, GatewaySerial, CreatedAt, UpdatedAt)
            VALUES (@Serial, @Model, @GatewaySerial, GETUTCDATE(), GETUTCDATE())
            """;
        await connection.ExecuteAsync(insertSql, devices, transaction);

        transaction.Commit();
    }
}
