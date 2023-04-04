using IBM.Application.Models.DTOs.Common;
using IBM.Application.Models.DTOs.Transactions;

namespace IBM.Application.Contracts.Services;

public interface ITransactionsService
{
    Task<PaginatedListDTO<TransactionDTO>> GetAllAsync(PaginationDTO pagination, CancellationToken cancellationToken = default);
    Task<SkuTransactionsDTO> GetBySkuAsync(string sku, string currency, CancellationToken cancellationToken = default);
}