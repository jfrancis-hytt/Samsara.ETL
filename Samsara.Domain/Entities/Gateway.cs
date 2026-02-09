namespace Samsara.Domain.Entities;

public class Gateway
{
    public string Serial { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string AssetId { get; set; } = string.Empty;
    public string SamsaraSerial { get; set; } = string.Empty;
    public string? SamsaraVin { get; set; }
    public string HealthStatus { get; set; } = string.Empty;
    public DateTime? LastConnected { get; set; }
    public long CellularDataUsageBytes { get; set; }
    public long HotspotUsageBytes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
