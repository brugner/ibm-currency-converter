using IBM.Application.Models.DTOs.Common;
using IBM.Domain.Entities;

namespace IBM.Application.Contracts.Repositories;

public interface ITransactionsRepository
{
    Task<IEnumerable<Transaction>> GetAllAsync(PaginationDTO pagination, CancellationToken cancellationToken);
    Task<IEnumerable<Transaction>> GetBySkuAsync(string sku, CancellationToken cancellationToken);
    Task<int> GetCountAsync(CancellationToken cancellationToken);
    Task UpdateAsync(IEnumerable<Transaction> transactions, CancellationToken cancellationToken);
}