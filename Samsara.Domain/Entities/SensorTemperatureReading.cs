namespace Samsara.Domain.Entities;

public class SensorTemperatureReading
{
    public long Id { get; set; }
    public long SensorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? AmbientTemperature { get; set; }
    public DateTime? AmbientTemperatureTime { get; set; }
    public int? ProbeTemperature { get; set; }
    public DateTime? ProbeTemperatureTime { get; set; }
    public long? TrailerId { get; set; }
    public DateTime CreatedAt { get; set; }
}
