using IBM.Application.Contracts.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace IBM.Infrastructure.Services;

public class ExternalDataService : IExternalDataService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ExternalDataService> _logger;

    public ExternalDataService(IHttpClientFactory httpClientFactory, ILogger<ExternalDataService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<T>> GetExternalDataAsync<T>(string providerUri, string resourceName, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching '{Resource}' from data provider '{Provider}'", resourceName, providerUri);

            using var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(providerUri, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var externalData = JsonSerializer.Deserialize<IEnumerable<T>>(content, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

            _logger.LogInformation("Resource '{Resource}' successfully fetched from data provider '{Provider}'. Count: {Count}", resourceName, providerUri, externalData.Count());

            return externalData;
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Failed to fetch '{Resource}' from data provider '{Provider}'. Provider response: {Response}", resourceName, providerUri, ex.Message);
            return Enumerable.Empty<T>();
        }
    }
}