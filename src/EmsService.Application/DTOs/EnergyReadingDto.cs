namespace EmsService.Application.DTOs;

public class EnergyReadingDto
{
    public string Id { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public decimal EnergyValue { get; set; }
    public string MeterId { get; set; } = string.Empty;
    public string LocationId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

