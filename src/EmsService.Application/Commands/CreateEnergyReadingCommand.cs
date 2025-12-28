using MediatR;
using EmsService.Application.Common.Models;

namespace EmsService.Application.Commands;

public class CreateEnergyReadingCommand : IRequest<Result<string>>
{
    public DateTime Timestamp { get; set; }
    public decimal EnergyValue { get; set; }
    public string MeterId { get; set; } = string.Empty;
    public string LocationId { get; set; } = string.Empty;
}
