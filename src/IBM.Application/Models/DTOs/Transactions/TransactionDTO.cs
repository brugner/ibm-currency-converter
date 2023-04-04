namespace IBM.Application.Models.DTOs.Transactions;

public class TransactionDTO
{
    public required string Sku { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }

    public override string ToString()
    {
        return $"Sku: {Sku}, Amount: {Amount}, Currency: {Currency}";
    }
}