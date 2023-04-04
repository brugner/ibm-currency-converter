using IBM.Application.Constants;
using IBM.Application.Contracts.Services;
using IBM.Application.Models.DTOs.Transactions;
using Swashbuckle.AspNetCore.Annotations;

namespace IBM.API.Endpoints.Transactions;

public static class GetTransactionsBySkuEndpoint
{
    public static IEndpointRouteBuilder MapGetTransactionsBySkuEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet(Endpoints.Transactions.GetBySku, async (string sku, string? currency, ITransactionsService transactionsService, CancellationToken cancellationToken) =>
        {
            currency ??= Currencies.EUR;
            var skuTransactions = await transactionsService.GetBySkuAsync(sku, currency, cancellationToken);

            return TypedResults.Ok(skuTransactions);
        })
        .Produces<SkuTransactionsDTO>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithMetadata(new SwaggerOperationAttribute("Get sku transactions", "Returns all the available transactions for the specified sku. All the amounts will be converted to the specified currency."));

        return builder;
    }
}
