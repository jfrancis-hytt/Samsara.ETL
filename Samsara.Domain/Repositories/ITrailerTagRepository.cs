using Samsara.Domain.Entities;

namespace Samsara.Domain.Repositories;

public interface ITrailerTagRepository
{
    Task ReplaceByTrailerAsync(string trailerId, IEnumerable<TrailerTag> tags);
}
