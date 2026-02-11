using Samsara.Domain.Entities;

namespace Samsara.Domain.Interfaces.Repositories;

public interface IGatewayRepository
{
    Task UpsertBatchAsync(IEnumerable<GatewayEntity> gateways);
    Task<IReadOnlyList<GatewayEntity>> GetAllAsync();
}
