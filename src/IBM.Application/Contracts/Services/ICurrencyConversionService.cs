using IBM.Application.Models.DTOs.Rates;
using IBM.Application.Models.DTOs.Transactions;

namespace IBM.Application.Contracts.Services;

public interface ICurrencyConversionService
{
    Task<IEnumerable<TransactionDTO>> ApplyRatesToTransactionsAsync(IEnumerable<RateDTO> rates, IEnumerable<TransactionDTO> transactions, string targetCurrency);
}