using IBM.Application.Constants;
using IBM.Domain.Entities;
using IBM.Infrastructure.Contracts.Services;
using IBM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IBM.Infrastructure.Services;

public class DbManagerService : IDbManagerService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DbManagerService> _logger;

    public DbManagerService(IServiceScopeFactory scopeFactory, ILogger<DbManagerService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public void Migrate()
    {
        using var serviceScope = _scopeFactory.CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<IBMDbContext>();

        if (context is null)
            throw new Exception("DbContext is null");

        foreach (var item in context.Database.GetPendingMigrations())
        {
            _logger.LogInformation("Applying pending migration: {item}", item);
        }

        context.Database.Migrate();
    }

    public void Seed()
    {
        using var serviceScope = _scopeFactory.CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<IBMDbContext>();

        if (context is null)
            throw new Exception("DbContext is null");

        SeedRates(context);
    }

    private void SeedRates(IBMDbContext context)
    {
        if (!context.Rates.Any())
        {
            _logger.LogInformation("Seeding rates..");

            var rates = GetRatesSeeds();

            context.Rates.AddRange(rates);
            context.SaveChanges();
        }
    }

    private static IEnumerable<Rate> GetRatesSeeds()
    {
        return new List<Rate>
        {
            new Rate { From = Currencies.EUR, To = Currencies.USD, Value = 1.359m },
            new Rate { From = Currencies.CAD, To = Currencies.EUR, Value = 0.732m },
            new Rate { From = Currencies.USD, To = Currencies.EUR, Value = 0.736m },
            new Rate { From = Currencies.EUR, To = Currencies.CAD, Value = 1.366m }
        };
    }
}