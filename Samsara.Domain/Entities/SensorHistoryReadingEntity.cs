namespace Samsara.Domain.Entities;

public class SensorHistoryReadingEntity
{
    public long Id { get; set; }
    public long SensorId { get; set; }
    public long TimeMs { get; set; }
    public int? ProbeTemperature { get; set; }
    public int? AmbientTemperature { get; set; }
    public DateTime CreatedAt { get; set; }
}
