using Dapper;
using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;

namespace Samsara.Infrastructure.Repositories;

public class TrailerTagRepository : ITrailerTagRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TrailerTagRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task ReplaceAllAsync(IEnumerable<TrailerTagEntity> tags)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = connection.BeginTransaction();

        const string deleteSql = "DELETE FROM [dbo].[TrailerTags]";
        await connection.ExecuteAsync(deleteSql, transaction: transaction);

        const string insertSql = """
            INSERT INTO [dbo].[TrailerTags] (Id, Name, ParentTagId, TrailerId, CreatedAt, UpdatedAt)
            VALUES (@Id, @Name, @ParentTagId, @TrailerId, SYSUTCDATETIME(), SYSUTCDATETIME())
            """;
        await connection.ExecuteAsync(insertSql, tags, transaction);

        transaction.Commit();
    }
}
