using MediatR;
using EmsService.Application.Common.Models;
using EmsService.Application.DTOs;
using EmsService.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmsService.Application.Queries;

public class GetEnergyReadingQueryHandler : IRequestHandler<GetEnergyReadingQuery, Result<EnergyReadingDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetEnergyReadingQueryHandler> _logger;

    public GetEnergyReadingQueryHandler(
        IApplicationDbContext context,
        ILogger<GetEnergyReadingQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<EnergyReadingDto>> Handle(GetEnergyReadingQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var energyReading = await _context.GetEnergyReadingByIdAsync(request.Id, cancellationToken);
            
            if (energyReading == null)
            {
                return Result.Failure<EnergyReadingDto>($"Energy reading with id {request.Id} not found");
            }

            var dto = new EnergyReadingDto
            {
                Id = energyReading.Id,
                Timestamp = energyReading.Timestamp,
                EnergyValue = energyReading.EnergyValue,
                MeterId = energyReading.MeterId,
                LocationId = energyReading.LocationId,
                CreatedAt = energyReading.CreatedAt,
                UpdatedAt = energyReading.UpdatedAt
            };

            return Result.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving energy reading {Id}", request.Id);
            return Result.Failure<EnergyReadingDto>("An error occurred while retrieving the energy reading");
        }
    }
}
