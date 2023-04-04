namespace IBM.Domain.Entities;

public class Transaction
{
    public int Id { get; set; }
    public required string Sku { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, Sku: {Sku}, Amount: {Amount}, Currency: {Currency}";
    }
}