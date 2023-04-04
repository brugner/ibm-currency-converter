using AutoMapper;
using IBM.Application.Contracts.Repositories;
using IBM.Application.Contracts.Services;
using IBM.Application.Exceptions;
using IBM.Application.Models.DTOs.Common;
using IBM.Application.Models.DTOs.Transactions;
using IBM.Application.Options;
using IBM.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IBM.Application.Services;

public class TransactionsService : ITransactionsService
{
    private readonly IMapper _mapper;
    private readonly ITransactionsRepository _transactionsRepository;
    private readonly ILogger<TransactionsService> _logger;
    private readonly IRatesService _ratesService;
    private readonly ICurrencyConversionService _currencyConversionService;
    private readonly IExternalDataService _externalDataService;
    private readonly DataProvidersOptions _dataProvidersOptions;

    public TransactionsService(IMapper mapper, ITransactionsRepository transactionsRepository, ILogger<TransactionsService> logger,
        IRatesService ratesService, ICurrencyConversionService currencyConversionService, IExternalDataService externalDataService,
        IOptions<DataProvidersOptions> dataProvidersOptions)
    {
        _mapper = mapper;
        _transactionsRepository = transactionsRepository;
        _logger = logger;
        _ratesService = ratesService;
        _currencyConversionService = currencyConversionService;
        _externalDataService = externalDataService;
        _dataProvidersOptions = dataProvidersOptions.Value;
    }

    public async Task<PaginatedListDTO<TransactionDTO>> GetAllAsync(PaginationDTO pagination, CancellationToken cancellationToken = default)
    {
        await UpdateTransactionsAsync(cancellationToken);

        _logger.LogInformation("Fetching transactions from database");

        var transactions = await _transactionsRepository.GetAllAsync(pagination, cancellationToken);
        var count = await _transactionsRepository.GetCountAsync(cancellationToken);

        return new PaginatedListDTO<TransactionDTO>
        {
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalItems = count,
            Items = _mapper.Map<IEnumerable<TransactionDTO>>(transactions)
        };
    }

    public async Task<SkuTransactionsDTO> GetBySkuAsync(string sku, string currency, CancellationToken cancellationToken)
    {
        currency = currency.ToUpperInvariant();

        var rates = await _ratesService.GetAllAsync(cancellationToken);

        if (!rates.Any(x => x.From.Equals(currency, StringComparison.InvariantCultureIgnoreCase)))
        {
            throw new InvalidCurrencyException($"Currency '{currency}' is invalid");
        }

        await UpdateTransactionsAsync(cancellationToken);

        _logger.LogInformation("Fetching transactions from database for sku '{Sku}'", sku);

        var transactions = await _transactionsRepository.GetBySkuAsync(sku, cancellationToken);
        var mappedTransactions = _mapper.Map<IEnumerable<TransactionDTO>>(transactions);

        mappedTransactions = await _currencyConversionService.ApplyRatesToTransactionsAsync(rates, mappedTransactions, currency);

        return new SkuTransactionsDTO
        {
            Sku = sku,
            Currency = currency,
            Transactions = _mapper.Map<IEnumerable<SimpleTransactionDTO>>(mappedTransactions)
        };
    }

    private async Task UpdateTransactionsAsync(CancellationToken cancellationToken)
    {
        var externalTransactions = await _externalDataService.GetExternalDataAsync<ExternalTransactionDTO>(_dataProvidersOptions.Transactions, "transactions", cancellationToken);
        var mappedTransactions = _mapper.Map<IEnumerable<Transaction>>(externalTransactions);

        if (mappedTransactions.Any())
        {
            await _transactionsRepository.UpdateAsync(mappedTransactions, cancellationToken);
            _logger.LogInformation("Transactions successfully updated");
        }
    }
}
