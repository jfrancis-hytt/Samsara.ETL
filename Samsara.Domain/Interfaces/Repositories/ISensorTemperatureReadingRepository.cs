using Samsara.Domain.Entities;

namespace Samsara.Domain.Interfaces.Repositories;

public interface ISensorTemperatureReadingRepository
{
    Task InsertBatchAsync(IEnumerable<SensorTemperatureReadingEntity> readings);
}
