using IBM.Domain.Entities;

namespace IBM.Application.Contracts.Repositories;

public interface IRatesRepository
{
    Task<IEnumerable<Rate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(IEnumerable<Rate> rates, CancellationToken cancellationToken = default);
}