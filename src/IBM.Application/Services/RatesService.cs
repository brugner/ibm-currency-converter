using AutoMapper;
using IBM.Application.Contracts.Repositories;
using IBM.Application.Contracts.Services;
using IBM.Application.Models.DTOs.Rates;
using IBM.Application.Options;
using IBM.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IBM.Application.Services;

public class RatesService : IRatesService
{
    private readonly IMapper _mapper;
    private readonly IRatesRepository _ratesRepository;
    private readonly ILogger<RatesService> _logger;
    private readonly IExternalDataService _externalDataService;
    private readonly DataProvidersOptions _dataProvidersOptions;

    public RatesService(IMapper mapper, IRatesRepository ratesRepository, ILogger<RatesService> logger,
        IExternalDataService externalDataService, IOptions<DataProvidersOptions> dataProvidersOptions)
    {
        _mapper = mapper;
        _ratesRepository = ratesRepository;
        _logger = logger;
        _externalDataService = externalDataService;
        _dataProvidersOptions = dataProvidersOptions.Value;
    }

    public async Task<IEnumerable<RateDTO>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var externalRates = await _externalDataService.GetExternalDataAsync<ExternalRateDTO>(_dataProvidersOptions.Rates, "rates", cancellationToken);
        var mappedRates = _mapper.Map<IEnumerable<Rate>>(externalRates);

        if (mappedRates.Any())
        {
            await _ratesRepository.UpdateAsync(mappedRates, cancellationToken);
            _logger.LogInformation("Rates successfully updated");
        }

        _logger.LogInformation("Fetching rates from database");

        var rates = await _ratesRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<RateDTO>>(rates);
    }
}