using Samsara.Domain.Entities;

namespace Samsara.Domain.Repositories;

public interface IGatewayRepository
{
    Task UpsertAsync(Gateway gateway);
    Task<IReadOnlyList<Gateway>> GetAllAsync();
}
