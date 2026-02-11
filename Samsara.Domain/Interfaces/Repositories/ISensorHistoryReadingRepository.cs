using Samsara.Domain.Entities;

namespace Samsara.Domain.Interfaces.Repositories;

public interface ISensorHistoryReadingRepository
{
    Task InsertAsync(SensorHistoryReading reading);
    Task InsertBatchAsync(IEnumerable<SensorHistoryReading> readings);
}
