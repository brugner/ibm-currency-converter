namespace IBM.Application.Contracts.Services;

public interface IExternalDataService
{
    Task<IEnumerable<T>> GetExternalDataAsync<T>(string providerUri, string resourceName, CancellationToken cancellationToken = default);
}