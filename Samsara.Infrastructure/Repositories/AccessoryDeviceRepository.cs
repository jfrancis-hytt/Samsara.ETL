using Dapper;
using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;

namespace Samsara.Infrastructure.Repositories;

public class AccessoryDeviceRepository : IAccessoryDeviceRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AccessoryDeviceRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task ReplaceAllAsync(IEnumerable<AccessoryDeviceEntity> devices)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = connection.BeginTransaction();

        const string deleteSql = "DELETE FROM [dbo].[AccessoryDevices]";
        await connection.ExecuteAsync(deleteSql, transaction: transaction);

        const string insertSql = """
            INSERT INTO [dbo].[AccessoryDevices] (Serial, Model, GatewaySerial, CreatedAt, UpdatedAt)
            VALUES (@Serial, @Model, @GatewaySerial, SYSUTCDATETIME(), SYSUTCDATETIME())
            """;
        await connection.ExecuteAsync(insertSql, devices, transaction);

        transaction.Commit();
    }
}
