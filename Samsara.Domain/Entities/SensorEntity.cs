namespace Samsara.Domain.Entities;

public class SensorEntity
{
    public long SensorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MacAddress { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
