namespace Samsara.Domain.Entities;

public class TrailerEntity
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? GatewaySerial { get; set; }
    public string? GatewayModel { get; set; }
    public string? SamsaraSerial { get; set; }
    public string? SamsaraVin { get; set; }
    public string? LicensePlate { get; set; }
    public string? Notes { get; set; }
    public bool EnabledForMobile { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
