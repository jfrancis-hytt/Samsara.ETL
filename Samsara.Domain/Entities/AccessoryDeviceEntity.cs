namespace Samsara.Domain.Entities;

public class AccessoryDeviceEntity
{
    public string Serial { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string GatewaySerial { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
