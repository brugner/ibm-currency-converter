using IBM.Application.Constants;
using IBM.Application.Models.DTOs.Rates;
using IBM.Application.Models.DTOs.Transactions;
using IBM.Domain.Entities;

namespace IBM.Unit.Tests.Fixtures;

public class BusinessDataFixture
{
    public IEnumerable<ExternalRateDTO> ExternalRates { get; } = new List<ExternalRateDTO>
    {
        new ExternalRateDTO { From = Currencies.EUR, To = Currencies.USD, Rate = 1.359m },
        new ExternalRateDTO { From = Currencies.CAD, To = Currencies.EUR, Rate = 0.732m },
        new ExternalRateDTO { From = Currencies.USD, To = Currencies.EUR, Rate = 0.736m },
        new ExternalRateDTO { From = Currencies.EUR, To = Currencies.CAD, Rate = 1.366m }
    };

    public IEnumerable<RateDTO> RatesDto { get; } = new List<RateDTO>
    {
        new RateDTO { From = Currencies.EUR, To = Currencies.USD, Rate = 1.359m },
        new RateDTO { From = Currencies.CAD, To = Currencies.EUR, Rate = 0.732m },
        new RateDTO { From = Currencies.USD, To = Currencies.EUR, Rate = 0.736m },
        new RateDTO { From = Currencies.EUR, To = Currencies.CAD, Rate = 1.366m }
    };

    public IEnumerable<Rate> Rates { get; } = new List<Rate>
    {
        new Rate { Id = 1, From = Currencies.EUR, To = Currencies.USD, Value = 1.359m },
        new Rate { Id = 2, From = Currencies.CAD, To = Currencies.EUR, Value = 0.732m },
        new Rate { Id = 3, From = Currencies.USD, To = Currencies.EUR, Value = 0.736m },
        new Rate { Id = 4, From = Currencies.EUR, To = Currencies.CAD, Value = 1.366m }
    };

    public IEnumerable<ExternalTransactionDTO> ExternalTransactions { get; } = new List<ExternalTransactionDTO>
    {
        new ExternalTransactionDTO { Sku = "T2006", Amount = 10, Currency = Currencies.USD },
        new ExternalTransactionDTO { Sku = "M2007", Amount = 34.57m, Currency = Currencies.CAD },
        new ExternalTransactionDTO { Sku = "R2008", Amount = 17.5m, Currency = Currencies.USD },
        new ExternalTransactionDTO { Sku = "T2006", Amount = 7.63m, Currency = Currencies.EUR },
        new ExternalTransactionDTO { Sku = "B2009", Amount = 21.23m, Currency = Currencies.USD }
    };

    public IEnumerable<Transaction> Transactions { get; } = new List<Transaction>
    {
        new Transaction { Id = 1, Sku = "T2006", Amount = 10, Currency = Currencies.USD },
        new Transaction { Id = 2, Sku = "M2007", Amount = 34.57m, Currency = Currencies.CAD },
        new Transaction { Id = 3, Sku = "R2008", Amount = 17.5m, Currency = Currencies.USD },
        new Transaction { Id = 4, Sku = "T2006", Amount = 7.63m, Currency = Currencies.EUR },
        new Transaction { Id = 5, Sku = "B2009", Amount = 21.23m, Currency = Currencies.USD }
    };

    public IEnumerable<TransactionDTO> TransactionsDto { get; } = new List<TransactionDTO>
    {
        new TransactionDTO { Sku = "T2006", Amount = 10, Currency = Currencies.USD },
        new TransactionDTO { Sku = "M2007", Amount = 34.57m, Currency = Currencies.CAD },
        new TransactionDTO { Sku = "R2008", Amount = 17.5m, Currency = Currencies.USD },
        new TransactionDTO { Sku = "T2006", Amount = 7.63m, Currency = Currencies.EUR },
        new TransactionDTO { Sku = "B2009", Amount = 21.23m, Currency = Currencies.USD }
    };
}