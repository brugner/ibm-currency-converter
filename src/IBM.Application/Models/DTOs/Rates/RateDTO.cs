namespace IBM.Application.Models.DTOs.Rates;

public class RateDTO
{
    public required string From { get; init; }
    public required string To { get; init; }
    public required decimal Rate { get; init; }

    public override string ToString()
    {
        return $"{From}->{To}: {Rate}";
    }
}