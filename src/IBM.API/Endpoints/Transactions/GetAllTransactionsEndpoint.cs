using IBM.Application.Contracts.Services;
using IBM.Application.Exceptions;
using IBM.Application.Models.DTOs.Common;
using IBM.Application.Models.DTOs.Transactions;
using Swashbuckle.AspNetCore.Annotations;

namespace IBM.API.Endpoints.Transactions;

public static class GetAllTransactionsEndpoint
{
    public static IEndpointRouteBuilder MapGetAllTransactionsEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet(Endpoints.Transactions.GetAll, async (int? page, int? pageSize, ITransactionsService transactionsService, CancellationToken cancellationToken) =>
        {
            var pagination = GetPaginationOrThrowIfInvalid(page, pageSize);
            var transactions = await transactionsService.GetAllAsync(pagination, cancellationToken);

            return TypedResults.Ok(transactions);
        })
        .Produces<PaginatedListDTO<TransactionDTO>>(StatusCodes.Status200OK)
        .WithMetadata(new SwaggerOperationAttribute("Get transactions", "Returns all the available transactions in a paginated list."));

        return builder;
    }

    private static PaginationDTO GetPaginationOrThrowIfInvalid(int? page, int? pageSize)
    {
        var pagination = new PaginationDTO { Page = page ?? PaginationDTO.DefaultPage, PageSize = pageSize ?? PaginationDTO.DefaultPageSize };

        if (pagination.PageSize < PaginationDTO.MinPageSize || pagination.PageSize > PaginationDTO.MaxPageSize)
        {
            throw new PaginationException($"Page size must be between {PaginationDTO.MinPageSize} and {PaginationDTO.MaxPageSize}.");
        }

        return pagination;
    }
}
