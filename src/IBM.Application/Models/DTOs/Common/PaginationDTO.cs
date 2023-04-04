namespace IBM.Application.Models.DTOs.Common;

public class PaginationDTO
{
    public const int MinPageSize = 1;
    public const int MaxPageSize = 100;
    public const int DefaultPage = 1;
    public const int DefaultPageSize = 10;

    public required int Page { get; init; } = DefaultPage;
    public required int PageSize { get; init; } = DefaultPageSize;
}