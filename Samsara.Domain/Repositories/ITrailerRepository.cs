using Samsara.Domain.Entities;

namespace Samsara.Domain.Repositories;

public interface ITrailerRepository
{
    Task UpsertAsync(Trailer trailer);
    Task<IReadOnlyList<Trailer>> GetAllAsync();
}
