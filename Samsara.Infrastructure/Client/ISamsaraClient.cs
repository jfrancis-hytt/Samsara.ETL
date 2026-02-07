using Samsara.Infrastructure.Responses;

namespace Samsara.Infrastructure.Client;

public interface ISamsaraClient
{
    Task<SensorResponse> GetSensorsAsync(CancellationToken ct = default);
}
