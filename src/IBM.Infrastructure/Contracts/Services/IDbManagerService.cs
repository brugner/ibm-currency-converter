namespace IBM.Infrastructure.Contracts.Services;

public interface IDbManagerService
{
    void Migrate();
    void Seed();
}