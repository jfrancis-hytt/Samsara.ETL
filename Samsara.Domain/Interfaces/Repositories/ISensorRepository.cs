using Samsara.Domain.Entities;

namespace Samsara.Domain.Interfaces.Repositories;

public interface ISensorRepository
{
    Task UpsertAsync(Sensor sensor);
    Task<IReadOnlyList<Sensor>> GetAllAsync();
}
