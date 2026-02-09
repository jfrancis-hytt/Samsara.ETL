using Samsara.Domain.Entities;

namespace Samsara.Domain.Interfaces.Repositories;

public interface IAccessoryDeviceRepository
{
    Task ReplaceByGatewayAsync(string gatewaySerial, IEnumerable<AccessoryDevice> devices);
}
