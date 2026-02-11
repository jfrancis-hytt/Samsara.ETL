using Samsara.Domain.Entities;

namespace Samsara.Domain.Interfaces.Repositories;

public interface IAccessoryDeviceRepository
{
    Task ReplaceAllAsync(IEnumerable<AccessoryDeviceEntity> devices);
}
