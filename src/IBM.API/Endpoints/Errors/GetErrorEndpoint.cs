using IBM.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace IBM.API.Endpoints.Errors;

public static class GetErrorEndpoint
{
    public static IEndpointRouteBuilder MapGetErrorEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.Map(Endpoints.Errors.GetError, (HttpContext httpContext, IWebHostEnvironment environment) =>
        {
            int code = StatusCodes.Status500InternalServerError;
            var context = httpContext.Features.Get<IExceptionHandlerFeature>()!;

            if (context.Error is InvalidCurrencyException or PaginationException)
                code = StatusCodes.Status400BadRequest;

            if (environment.IsDevelopment())
            {
                return Results.Problem(statusCode: code, title: context.Error.Message, instance: context.Path, detail: context.Error.StackTrace);
            }
            else
            {
                return Results.Problem(statusCode: code, title: context.Error.Message, instance: context.Path);
            }

        })
        .ExcludeFromDescription();

        return builder;
    }
}
