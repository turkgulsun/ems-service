using MediatR;
using EmsService.Application.Common.Models;
using EmsService.Application.Interfaces;
using EmsService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EmsService.Application.Commands;

public class CreateEnergyReadingCommandHandler : IRequestHandler<CreateEnergyReadingCommand, Result<string>>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CreateEnergyReadingCommandHandler> _logger;

    public CreateEnergyReadingCommandHandler(
        IApplicationDbContext context,
        ILogger<CreateEnergyReadingCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(CreateEnergyReadingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var energyReadingId = Guid.NewGuid().ToString();
            var energyReading = new EnergyReading(
                id: energyReadingId,
                timestamp: request.Timestamp,
                energyValue: request.EnergyValue,
                meterId: request.MeterId,
                locationId: request.LocationId);

            var outboxMessageId = Guid.NewGuid().ToString();
            var outboxMessage = new OutboxMessage(
                id: outboxMessageId,
                eventType: "EnergyReadingCreated",
                payload: System.Text.Json.JsonSerializer.Serialize(new 
                { 
                    EnergyReadingId = energyReadingId, 
                    Timestamp = energyReading.Timestamp 
                }));

            var id = await _context.CreateEnergyReadingWithOutboxAsync(
                energyReading, 
                outboxMessage, 
                cancellationToken);

            return Result.Success(id);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error while creating energy reading");
            return Result.Failure<string>(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating energy reading");
            return Result.Failure<string>("An error occurred while creating the energy reading");
        }
    }
}
