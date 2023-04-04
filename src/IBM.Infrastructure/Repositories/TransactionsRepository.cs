using IBM.Application.Contracts.Repositories;
using IBM.Application.Models.DTOs.Common;
using IBM.Domain.Entities;
using IBM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IBM.Infrastructure.Repositories;

public class TransactionsRepository : ITransactionsRepository
{
    private readonly IBMDbContext _dbContext;

    public TransactionsRepository(IBMDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync(PaginationDTO pagination, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Transactions
            .OrderBy(x => x.Id)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetBySkuAsync(string sku, CancellationToken cancellationToken)
    {
        sku = sku.ToUpperInvariant();

        return await _dbContext.Transactions
            .Where(x => x.Sku == sku)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Transactions.CountAsync(cancellationToken);
    }

    public async Task UpdateAsync(IEnumerable<Transaction> transactions, CancellationToken cancellationToken = default)
    {
        foreach (var item in transactions)
        {
            item.Sku = item.Sku.ToUpperInvariant();
        }

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        await _dbContext.Transactions.ExecuteDeleteAsync(cancellationToken);
        await _dbContext.Transactions.AddRangeAsync(transactions, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }
}