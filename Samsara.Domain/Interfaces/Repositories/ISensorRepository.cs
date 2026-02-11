using Samsara.Domain.Entities;

namespace Samsara.Domain.Interfaces.Repositories;

public interface ISensorRepository
{
    Task UpsertBatchAsync(IEnumerable<SensorEntity> sensors);
    Task<IReadOnlyList<SensorEntity>> GetAllAsync();
}
