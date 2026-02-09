using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;

namespace Samsara.Infrastructure.Repositories;

public class TrailerRepository : ITrailerRepository
{
    private readonly string _connectionString;

    public TrailerRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SamsaraDbConnectionString")!;
    }

    public async Task UpsertAsync(Trailer trailer)
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
                    UpdatedAt = GETUTCDATE()
            WHEN NOT MATCHED THEN
                INSERT (Id, Name, GatewaySerial, GatewayModel, SamsaraSerial, SamsaraVin,
                        LicensePlate, Notes, EnabledForMobile, CreatedAt, UpdatedAt)
                VALUES (@Id, @Name, @GatewaySerial, @GatewayModel, @SamsaraSerial, @SamsaraVin,
                        @LicensePlate, @Notes, @EnabledForMobile, GETUTCDATE(), GETUTCDATE());
            """;

        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, trailer);
    }

    public async Task<IReadOnlyList<Trailer>> GetAllAsync()
    {
        const string sql = """
            SELECT Id, Name, GatewaySerial, GatewayModel, SamsaraSerial, SamsaraVin,
                   LicensePlate, Notes, EnabledForMobile, CreatedAt, UpdatedAt
            FROM [dbo].[Trailers]
            """;

        using var connection = new SqlConnection(_connectionString);
        var results = await connection.QueryAsync<Trailer>(sql);
        return results.ToList();
    }
}
