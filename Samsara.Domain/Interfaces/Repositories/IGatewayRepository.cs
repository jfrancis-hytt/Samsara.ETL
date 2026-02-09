using Samsara.Domain.Entities;

namespace Samsara.Domain.Interfaces.Repositories;

public interface IGatewayRepository
{
    Task UpsertAsync(Gateway gateway);
    Task<IReadOnlyList<Gateway>> GetAllAsync();
}
