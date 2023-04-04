namespace IBM.Domain.Entities;

public class Rate
{
    public int Id { get; set; }
    public required string From { get; set; }
    public required string To { get; set; }
    public required decimal Value { get; set; }
}