using Samsara.Domain.Entities;

namespace Samsara.Domain.Repositories;

public interface IAccessoryDeviceRepository
{
    Task ReplaceByGatewayAsync(string gatewaySerial, IEnumerable<AccessoryDevice> devices);
}
