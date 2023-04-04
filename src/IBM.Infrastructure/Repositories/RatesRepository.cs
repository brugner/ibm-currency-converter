using IBM.Application.Contracts.Repositories;
using IBM.Domain.Entities;
using IBM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IBM.Infrastructure.Repositories;

public class RatesRepository : IRatesRepository
{
    private readonly IBMDbContext _dbContext;

    public RatesRepository(IBMDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Rate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Rates.ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(IEnumerable<Rate> rates, CancellationToken cancellationToken = default)
    {
        foreach (var rate in rates)
        {
            rate.From = rate.From.ToUpperInvariant();
            rate.To = rate.To.ToUpperInvariant();
        }

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        await _dbContext.Rates.ExecuteDeleteAsync(cancellationToken);
        await _dbContext.Rates.AddRangeAsync(rates, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }
}