using IBM.Application.Contracts.Repositories;
using IBM.Application.Contracts.Services;
using IBM.Infrastructure.Contracts.Services;
using IBM.Infrastructure.Data;
using IBM.Infrastructure.Repositories;
using IBM.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IBM.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<IBMDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddTransient<IRatesRepository, RatesRepository>();
        services.AddTransient<ITransactionsRepository, TransactionsRepository>();

        services.AddSingleton<IDbManagerService, DbManagerService>();
        services.AddSingleton<IExternalDataService, ExternalDataService>();
    }
}