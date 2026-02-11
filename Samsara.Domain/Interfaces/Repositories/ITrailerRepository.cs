using Samsara.Domain.Entities;

namespace Samsara.Domain.Interfaces.Repositories;

public interface ITrailerRepository
{
    Task UpsertBatchAsync(IEnumerable<TrailerEntity> trailers);
    Task<IReadOnlyList<TrailerEntity>> GetAllAsync();
}
