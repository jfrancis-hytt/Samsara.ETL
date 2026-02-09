using Samsara.Domain.Entities;

namespace Samsara.Domain.Repositories;

public interface ISensorTemperatureReadingRepository
{
    Task InsertAsync(SensorTemperatureReading reading);
}
