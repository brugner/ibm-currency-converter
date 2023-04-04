using IBM.Application.Models.DTOs.Rates;

namespace IBM.Application.Contracts.Services;

public interface IRatesService
{
    Task<IEnumerable<RateDTO>> GetAllAsync(CancellationToken cancellationToken = default);
}