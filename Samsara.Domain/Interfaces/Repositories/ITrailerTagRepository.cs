using Samsara.Domain.Entities;

namespace Samsara.Domain.Interfaces.Repositories;

public interface ITrailerTagRepository
{
    Task ReplaceAllAsync(IEnumerable<TrailerTagEntity> tags);
}
