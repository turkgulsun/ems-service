using EmsService.Domain.Entities;

namespace EmsService.Domain.Repositories;

public interface IEnergyReadingRepository
{
    Task<EnergyReading?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<EnergyReading>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<string> CreateAsync(EnergyReading energyReading, CancellationToken cancellationToken = default);
    Task UpdateAsync(EnergyReading energyReading, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}

