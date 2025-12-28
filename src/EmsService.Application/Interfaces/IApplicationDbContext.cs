using EmsService.Domain.Entities;

namespace EmsService.Application.Interfaces;

public interface IApplicationDbContext
{
    Task<string> CreateEnergyReadingWithOutboxAsync(
        EnergyReading energyReading, 
        OutboxMessage outboxMessage, 
        CancellationToken cancellationToken = default);
    Task<EnergyReading?> GetEnergyReadingByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<EnergyReading>> GetAllEnergyReadingsAsync(CancellationToken cancellationToken = default);
}

