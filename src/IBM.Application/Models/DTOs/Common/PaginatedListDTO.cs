namespace IBM.Application.Models.DTOs.Common;

public class PaginatedListDTO<T>
{
    public required int Page { get; set; }
    public required int PageSize { get; set; }
    public required int TotalItems { get; set; }
    public required IEnumerable<T> Items { get; set; }
}