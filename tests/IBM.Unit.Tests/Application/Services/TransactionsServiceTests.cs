using AutoMapper;
using IBM.Application.Constants;
using IBM.Application.Contracts.Repositories;
using IBM.Application.Contracts.Services;
using IBM.Application.Exceptions;
using IBM.Application.MappingProfiles;
using IBM.Application.Models.DTOs.Common;
using IBM.Application.Models.DTOs.Transactions;
using IBM.Application.Options;
using IBM.Application.Services;
using IBM.Domain.Entities;
using IBM.Unit.Tests.Fixtures;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace IBM.Unit.Tests.Application.Services;

public class TransactionsServiceTests : IClassFixture<BusinessDataFixture>
{
    private readonly TransactionsService _sut;
    private readonly BusinessDataFixture _fixture;
    private readonly IMapper _mapper;
    private readonly Mock<ITransactionsRepository> _transactionsRepositoryMock;
    private readonly NullLogger<TransactionsService> _loggerMock;
    private readonly Mock<IRatesService> _ratesServiceMock;
    private readonly Mock<ICurrencyConversionService> _currencyConversionServiceMock;
    private readonly Mock<IExternalDataService> _externalDataServiceMock;
    private readonly IOptions<DataProvidersOptions> _dataProvidersOptions;

    public TransactionsServiceTests(BusinessDataFixture fixture)
    {
        _fixture = fixture;

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new TransactionMappingProfile());
        });

        _mapper = mapperConfig.CreateMapper();
        _transactionsRepositoryMock = new Mock<ITransactionsRepository>();
        _loggerMock = new NullLogger<TransactionsService>();
        _ratesServiceMock = new Mock<IRatesService>();
        _currencyConversionServiceMock = new Mock<ICurrencyConversionService>();
        _externalDataServiceMock = new Mock<IExternalDataService>();
        _dataProvidersOptions = Options.Create(new DataProvidersOptions { Rates = "rates-url", Transactions = "transactions-url" });

        _sut = new TransactionsService(_mapper, _transactionsRepositoryMock.Object, _loggerMock, _ratesServiceMock.Object,
            _currencyConversionServiceMock.Object, _externalDataServiceMock.Object, _dataProvidersOptions);
    }

    [Fact]
    public async Task GetAllAsync_ExternalProviderOk_ReturnsTransactions()
    {
        // Arrange
        _externalDataServiceMock.Setup(x => x.GetExternalDataAsync<ExternalTransactionDTO>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.ExternalTransactions)
            .Verifiable();

        _transactionsRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<IEnumerable<Transaction>>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var pagination = new PaginationDTO { Page = 1, PageSize = 3 };

        _transactionsRepositoryMock.Setup(x => x.GetAllAsync(pagination, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Transactions.Take(pagination.PageSize))
            .Verifiable();

        _transactionsRepositoryMock.Setup(x => x.GetCountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Transactions.Count())
            .Verifiable();

        // Act
        var result = await _sut.GetAllAsync(pagination, It.IsAny<CancellationToken>());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_fixture.Transactions.Count(), result.TotalItems);
        Assert.Equal(pagination.PageSize, result.Items.Count());


        _externalDataServiceMock.Verify(x => x.GetExternalDataAsync<ExternalTransactionDTO>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _transactionsRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<IEnumerable<Transaction>>(), It.IsAny<CancellationToken>()), Times.Once);
        _transactionsRepositoryMock.Verify(x => x.GetAllAsync(pagination, It.IsAny<CancellationToken>()), Times.Once);
        _transactionsRepositoryMock.Verify(x => x.GetCountAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ExternalProviderReturnsEmptyTransactions_ReturnsTransactions()
    {
        // Arrange
        _externalDataServiceMock.Setup(x => x.GetExternalDataAsync<ExternalTransactionDTO>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<ExternalTransactionDTO>())
            .Verifiable();

        _transactionsRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<IEnumerable<Transaction>>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var pagination = new PaginationDTO { Page = 1, PageSize = 3 };

        _transactionsRepositoryMock.Setup(x => x.GetAllAsync(pagination, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Transactions.Take(pagination.PageSize))
            .Verifiable();

        _transactionsRepositoryMock.Setup(x => x.GetCountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Transactions.Count())
            .Verifiable();

        // Act
        var result = await _sut.GetAllAsync(pagination, It.IsAny<CancellationToken>());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_fixture.Transactions.Count(), result.TotalItems);
        Assert.Equal(pagination.PageSize, result.Items.Count());


        _externalDataServiceMock.Verify(x => x.GetExternalDataAsync<ExternalTransactionDTO>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _transactionsRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<IEnumerable<Transaction>>(), It.IsAny<CancellationToken>()), Times.Never);
        _transactionsRepositoryMock.Verify(x => x.GetAllAsync(pagination, It.IsAny<CancellationToken>()), Times.Once);
        _transactionsRepositoryMock.Verify(x => x.GetCountAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetBySkuAsync_ValidCurrency_ReturnsSkuTransactions()
    {
        // Arrange
        string sku = "T2006";
        string currency = Currencies.EUR;
        decimal total = 14.99m;
        var transactionsForSku = _fixture.Transactions.Where(x => x.Sku == sku);

        _ratesServiceMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.RatesDto);

        _externalDataServiceMock.Setup(x => x.GetExternalDataAsync<ExternalTransactionDTO>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<ExternalTransactionDTO>())
            .Verifiable();

        _transactionsRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<IEnumerable<Transaction>>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _transactionsRepositoryMock.Setup(x => x.GetBySkuAsync(sku, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionsForSku);

        _currencyConversionServiceMock.Setup(x => x.ApplyRatesToTransactionsAsync(_fixture.RatesDto, It.IsAny<IEnumerable<TransactionDTO>>(), currency))
            .ReturnsAsync(new List<TransactionDTO>
            {
                new TransactionDTO { Sku = "T2006", Amount = 7.36m, Currency = Currencies.EUR },
                new TransactionDTO { Sku = "T2006", Amount = 7.63m, Currency = Currencies.EUR }
            })
            .Verifiable();

        // Act
        var result = await _sut.GetBySkuAsync(sku, currency, It.IsAny<CancellationToken>());

        // Assert
        Assert.Equal(sku, result.Sku);
        Assert.Equal(currency, result.Currency);
        Assert.Equal(total, result.Total);

        _ratesServiceMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once());
        _externalDataServiceMock.Verify(x => x.GetExternalDataAsync<ExternalTransactionDTO>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        _transactionsRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<IEnumerable<Transaction>>(), It.IsAny<CancellationToken>()), Times.Never);
        _transactionsRepositoryMock.Verify(x => x.GetBySkuAsync(sku, It.IsAny<CancellationToken>()), Times.Once());
        _currencyConversionServiceMock.Verify(x => x.ApplyRatesToTransactionsAsync(_fixture.RatesDto, It.IsAny<IEnumerable<TransactionDTO>>(), currency), Times.Once());
    }

    [Fact]
    public async Task GetBySkuAsync_InvalidCurrency_ThrowsInvalidCurrencyException()
    {
        // Arrange
        string sku = "T2006";
        string currency = "INX";

        _ratesServiceMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.RatesDto);

        // Act
        var result = await Record.ExceptionAsync(async () => await _sut.GetBySkuAsync(sku, currency, It.IsAny<CancellationToken>()));

        // Assert
        Assert.IsType<InvalidCurrencyException>(result);
        Assert.Equal($"Currency '{currency}' is invalid", result.Message);

        _ratesServiceMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}