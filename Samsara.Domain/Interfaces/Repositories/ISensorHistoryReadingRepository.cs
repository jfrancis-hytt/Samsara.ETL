using Samsara.Domain.Entities;

namespace Samsara.Domain.Interfaces.Repositories;

public interface ISensorHistoryReadingRepository
{
    Task InsertBatchAsync(IEnumerable<SensorHistoryReadingEntity> readings);
}
