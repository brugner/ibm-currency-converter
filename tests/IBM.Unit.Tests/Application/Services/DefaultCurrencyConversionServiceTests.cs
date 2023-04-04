using IBM.Application.Models.DTOs.Rates;
using IBM.Application.Models.DTOs.Transactions;
using IBM.Application.Services;
using IBM.Unit.Tests.TestData;
using Microsoft.Extensions.Logging.Abstractions;

namespace IBM.Unit.Tests.Application.Services;

public class DefaultCurrencyConversionServiceTests
{
    private readonly DefaultCurrencyConversionService _sut;

    public DefaultCurrencyConversionServiceTests()
    {
        _sut = new DefaultCurrencyConversionService(new NullLogger<DefaultCurrencyConversionService>());
    }

    [Theory]
    [ClassData(typeof(CurrencyConversionTestData))]
    public async Task ApplyRatesToTransactionsAsync_MultipleScenarios_CorrectConversion(IEnumerable<RateDTO> rates, IEnumerable<TransactionDTO> transactions, string currency, IEnumerable<TransactionDTO> expected)
    {
        // Act
        var result = await _sut.ApplyRatesToTransactionsAsync(rates, transactions, currency);

        // Assert
        Assert.Equal(expected.Count(), result.Count());
        Assert.Single(result.Select(x => x.Currency).Distinct());
        Assert.Equal(result.Select(x => x.Currency).Distinct().First(), currency);

        Assert.Equal(expected.First().Sku, result.First().Sku);
        Assert.Equal(expected.First().Amount, result.First().Amount);
        Assert.Equal(expected.First().Currency, result.First().Currency);

        Assert.Equal(expected.Last().Sku, result.Last().Sku);
        Assert.Equal(expected.Last().Amount, result.Last().Amount);
        Assert.Equal(expected.Last().Currency, result.Last().Currency);

        var random = new Random().Next(0, expected.Count());
        Assert.Equal(expected.ElementAt(random).Sku, result.ElementAt(random).Sku);
        Assert.Equal(expected.ElementAt(random).Amount, result.ElementAt(random).Amount);
        Assert.Equal(expected.ElementAt(random).Currency, result.ElementAt(random).Currency);
    }
}