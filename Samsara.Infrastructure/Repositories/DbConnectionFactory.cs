using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Samsara.Infrastructure.Repositories;

public interface IDbConnectionFactory
{
    SqlConnection CreateConnection();
}

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SamsaraDbConnectionString")
            ?? throw new InvalidOperationException("Connection string 'SamsaraDbConnectionString' is not configured.");
    }

    public SqlConnection CreateConnection() => new(_connectionString);
}
