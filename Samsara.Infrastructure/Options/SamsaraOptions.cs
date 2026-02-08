using System.ComponentModel.DataAnnotations;

namespace Samsara.Infrastructure.Options;

public class SamsaraOptions
{
    [Required]
    public string BaseUrl { get; set; } = string.Empty;

    [Required]
    public string ApiKey { get; set; } = string.Empty;
}