namespace EmsService.Application.DTOs;

public class CreateEnergyReadingDto
{
    public DateTime Timestamp { get; set; }
    public decimal EnergyValue { get; set; }
    public string MeterId { get; set; } = string.Empty;
    public string LocationId { get; set; } = string.Empty;
}

