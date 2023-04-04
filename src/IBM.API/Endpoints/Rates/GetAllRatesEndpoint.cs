using IBM.Application.Contracts.Services;
using IBM.Application.Models.DTOs.Rates;
using Swashbuckle.AspNetCore.Annotations;

namespace IBM.API.Endpoints.Rates;

public static class GetAllRatesEndpoint
{
    public static IEndpointRouteBuilder MapGetRatesEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet(Endpoints.Rates.GetAll, async (IRatesService ratesService, CancellationToken cancellationToken) =>
        {
            var rates = await ratesService.GetAllAsync(cancellationToken);

            return TypedResults.Ok(rates);
        })
        .Produces<IEnumerable<RateDTO>>(StatusCodes.Status200OK)
        .WithMetadata(new SwaggerOperationAttribute("Get rates", "Returns all the available currency rates."));

        return builder;
    }
}
