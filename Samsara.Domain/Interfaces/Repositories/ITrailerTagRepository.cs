using Samsara.Domain.Entities;

namespace Samsara.Domain.Interfaces.Repositories;

public interface ITrailerTagRepository
{
    Task ReplaceByTrailerAsync(string trailerId, IEnumerable<TrailerTag> tags);
}
