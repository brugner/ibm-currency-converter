namespace IBM.Application.Models.DTOs.Transactions;

public class SkuTransactionsDTO
{
    public required string Sku { get; set; }
    public required string Currency { get; set; }
    public required IEnumerable<SimpleTransactionDTO> Transactions { get; set; }
    public decimal Total => Transactions.Sum(x => x.Amount);
}