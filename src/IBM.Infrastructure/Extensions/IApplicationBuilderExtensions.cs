using IBM.Infrastructure.Contracts.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace IBM.Infrastructure.Extensions;

public static class IApplicationBuilderExtensions
{
    public static void UpdateDatabase(this IApplicationBuilder app, bool isDevelopment)
    {
        var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

        using var scope = scopeFactory.CreateScope();
        var dbInitializer = scope.ServiceProvider.GetService<IDbManagerService>();

        dbInitializer?.Migrate();

        if (isDevelopment)
        {
            dbInitializer?.Seed();
        }
    }
}