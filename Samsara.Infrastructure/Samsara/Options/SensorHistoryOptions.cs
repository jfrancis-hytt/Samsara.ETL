using System.ComponentModel.DataAnnotations;

namespace Samsara.Infrastructure.Samsara.Options;

public sealed class SensorHistoryOptions
{
    [Required]
    [Range(1, int.MaxValue)]
    public int LookbackHours { get; set; }

    [Required]
    [Range(1, long.MaxValue)]
    public long StepMs { get; set; }
}
