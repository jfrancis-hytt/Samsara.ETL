using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;

namespace Samsara.Infrastructure.Repositories;

public class TrailerTagRepository : ITrailerTagRepository
{
    private readonly string _connectionString;

    public TrailerTagRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SamsaraDbConnectionString")!;
    }

    public async Task ReplaceByTrailerAsync(string trailerId, IEnumerable<TrailerTag> tags)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        using var transaction = connection.BeginTransaction();

        const string deleteSql = "DELETE FROM [dbo].[TrailerTags] WHERE TrailerId = @TrailerId";
        await connection.ExecuteAsync(deleteSql, new { TrailerId = trailerId }, transaction);

        const string insertSql = """
            INSERT INTO [dbo].[TrailerTags] (Id, Name, ParentTagId, TrailerId, CreatedAt, UpdatedAt)
            VALUES (@Id, @Name, @ParentTagId, @TrailerId, GETUTCDATE(), GETUTCDATE())
            """;
        await connection.ExecuteAsync(insertSql, tags, transaction);

        transaction.Commit();
    }
}
