using IBM.API.Endpoints.Errors;
using IBM.API.Endpoints.Rates;
using IBM.API.Endpoints.Transactions;

namespace IBM.API.Endpoints;

public static class Endpoints
{
    private const string ApiBase = "api";

    public class Rates
    {
        private const string Base = $"{ApiBase}/rates";
        public const string GetAll = Base;
    }

    public class Transactions
    {
        private const string Base = $"{ApiBase}/transactions";
        public const string GetAll = Base;
        public const string GetBySku = $"{Base}/{{sku}}";
    }

    public class Errors
    {
        public const string GetError = $"{ApiBase}/errors";
    }

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGetRatesEndpoint();
        builder.MapGetAllTransactionsEndpoint();
        builder.MapGetTransactionsBySkuEndpoint();
        builder.MapGetErrorEndpoint();

        return builder;
    }
}
