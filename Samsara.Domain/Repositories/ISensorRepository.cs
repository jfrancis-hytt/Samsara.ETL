using Samsara.Domain.Entities;

namespace Samsara.Domain.Repositories;

public interface ISensorRepository
{
    Task UpsertAsync(Sensor sensor);
    Task<IReadOnlyList<Sensor>> GetAllAsync();
}
