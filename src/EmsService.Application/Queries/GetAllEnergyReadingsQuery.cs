using MediatR;
using EmsService.Application.Common.Models;
using EmsService.Application.DTOs;

namespace EmsService.Application.Queries;

public class GetAllEnergyReadingsQuery : IRequest<Result<IEnumerable<EnergyReadingDto>>>
{
}
