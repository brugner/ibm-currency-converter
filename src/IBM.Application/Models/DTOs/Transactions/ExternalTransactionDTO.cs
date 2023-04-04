namespace IBM.Application.Models.DTOs.Transactions;

public class ExternalTransactionDTO
{
    public required string Sku { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
}