using Samsara.Domain.Entities;

namespace Samsara.Domain.Interfaces.Repositories;

public interface ITrailerRepository
{
    Task UpsertAsync(Trailer trailer);
    Task<IReadOnlyList<Trailer>> GetAllAsync();
}
