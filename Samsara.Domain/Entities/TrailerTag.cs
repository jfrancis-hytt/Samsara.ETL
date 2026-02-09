namespace Samsara.Domain.Entities;

public class TrailerTag
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ParentTagId { get; set; } = string.Empty;
    public string TrailerId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
