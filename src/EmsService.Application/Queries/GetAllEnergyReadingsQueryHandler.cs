using MediatR;
using EmsService.Application.Common.Models;
using EmsService.Application.DTOs;
using EmsService.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmsService.Application.Queries;

public class GetAllEnergyReadingsQueryHandler : IRequestHandler<GetAllEnergyReadingsQuery, Result<IEnumerable<EnergyReadingDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetAllEnergyReadingsQueryHandler> _logger;

    public GetAllEnergyReadingsQueryHandler(
        IApplicationDbContext context,
        ILogger<GetAllEnergyReadingsQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<EnergyReadingDto>>> Handle(GetAllEnergyReadingsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var energyReadings = await _context.GetAllEnergyReadingsAsync(cancellationToken);

            var dtos = energyReadings.Select(er => new EnergyReadingDto
            {
                Id = er.Id,
                Timestamp = er.Timestamp,
                EnergyValue = er.EnergyValue,
                MeterId = er.MeterId,
                LocationId = er.LocationId,
                CreatedAt = er.CreatedAt,
                UpdatedAt = er.UpdatedAt
            });

            return Result.Success<IEnumerable<EnergyReadingDto>>(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving energy readings");
            return Result.Failure<IEnumerable<EnergyReadingDto>>("An error occurred while retrieving energy readings");
        }
    }
}
