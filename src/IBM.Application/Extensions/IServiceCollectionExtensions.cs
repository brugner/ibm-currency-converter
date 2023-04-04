using IBM.Application.Contracts.Services;
using IBM.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IBM.Application.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IRatesService, RatesService>();
        services.AddTransient<ITransactionsService, TransactionsService>();
        services.AddTransient<ICurrencyConversionService, DefaultCurrencyConversionService>();

        services.AddAutoMapper(typeof(IApplicationMarker).Assembly);

        return services;
    }
}