namespace IBM.Application.Models.DTOs.Rates;

public class ExternalRateDTO
{
    public required string From { get; init; }
    public required string To { get; init; }
    public required decimal Rate { get; init; }
}