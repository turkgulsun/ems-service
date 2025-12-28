using MediatR;
using EmsService.Application.Common.Models;
using EmsService.Application.DTOs;

namespace EmsService.Application.Queries;

public class GetEnergyReadingQuery : IRequest<Result<EnergyReadingDto>>
{
    public string Id { get; set; } = string.Empty;
}
