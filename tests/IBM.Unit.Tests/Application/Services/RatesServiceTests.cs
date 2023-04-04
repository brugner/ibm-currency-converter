using AutoMapper;
using IBM.Application.Contracts.Repositories;
using IBM.Application.Contracts.Services;
using IBM.Application.MappingProfiles;
using IBM.Application.Models.DTOs.Rates;
using IBM.Application.Options;
using IBM.Application.Services;
using IBM.Domain.Entities;
using IBM.Unit.Tests.Fixtures;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace IBM.Unit.Tests.Application.Services;

public class RatesServiceTests : IClassFixture<BusinessDataFixture>
{
    private readonly RatesService _sut;
    private readonly BusinessDataFixture _fixture;
    private readonly IMapper _mapper;
    private readonly Mock<IRatesRepository> _ratesRepositoryMock;
    private readonly NullLogger<RatesService> _loggerMock;
    private readonly Mock<IExternalDataService> _externalDataServiceMock;
    private readonly IOptions<DataProvidersOptions> _dataProvidersOptions;

    public RatesServiceTests(BusinessDataFixture fixture)
    {
        _fixture = fixture;

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new RateMappingProfile());
        });

        _mapper = mapperConfig.CreateMapper();
        _ratesRepositoryMock = new Mock<IRatesRepository>();
        _loggerMock = new NullLogger<RatesService>();
        _externalDataServiceMock = new Mock<IExternalDataService>();
        _dataProvidersOptions = Options.Create(new DataProvidersOptions { Rates = "rates-url", Transactions = "transactions-url" });

        _sut = new RatesService(_mapper, _ratesRepositoryMock.Object, _loggerMock, _externalDataServiceMock.Object, _dataProvidersOptions);
    }

    [Fact]
    public async Task GetAllAsync_ExternalProviderOk_ReturnsRates()
    {
        // Arrange
        _externalDataServiceMock.Setup(x => x.GetExternalDataAsync<ExternalRateDTO>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.ExternalRates)
            .Verifiable();

        _ratesRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<IEnumerable<Rate>>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _ratesRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Rates)
            .Verifiable();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_fixture.Rates.Count(), result.Count());

        Assert.Collection(
            result,
            item => Assert.Equal(_fixture.RatesDto.ElementAt(0).Rate, item.Rate),
            item => Assert.Equal(_fixture.RatesDto.ElementAt(1).Rate, item.Rate),
            item => Assert.Equal(_fixture.RatesDto.ElementAt(2).Rate, item.Rate),
            item => Assert.Equal(_fixture.RatesDto.ElementAt(3).Rate, item.Rate)
        );

        _externalDataServiceMock.Verify(x => x.GetExternalDataAsync<ExternalRateDTO>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _ratesRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<IEnumerable<Rate>>(), It.IsAny<CancellationToken>()), Times.Once);
        _ratesRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ExternalProviderReturnsEmptyRates_ReturnsRates()
    {
        // Arrange
        _externalDataServiceMock.Setup(x => x.GetExternalDataAsync<ExternalRateDTO>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<ExternalRateDTO>())
            .Verifiable();

        _ratesRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<IEnumerable<Rate>>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _ratesRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Rates)
            .Verifiable();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_fixture.Rates.Count(), result.Count());

        Assert.Collection(
            result,
            item => Assert.Equal(_fixture.RatesDto.ElementAt(0).Rate, item.Rate),
            item => Assert.Equal(_fixture.RatesDto.ElementAt(1).Rate, item.Rate),
            item => Assert.Equal(_fixture.RatesDto.ElementAt(2).Rate, item.Rate),
            item => Assert.Equal(_fixture.RatesDto.ElementAt(3).Rate, item.Rate)
        );

        _externalDataServiceMock.Verify(x => x.GetExternalDataAsync<ExternalRateDTO>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _ratesRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<IEnumerable<Rate>>(), It.IsAny<CancellationToken>()), Times.Never);
        _ratesRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}