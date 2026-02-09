using Samsara.Domain.Entities;

namespace Samsara.Domain.Interfaces.Repositories;

public interface ISensorTemperatureReadingRepository
{
    Task InsertAsync(SensorTemperatureReading reading);
}
