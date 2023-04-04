var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/external-api/v1/rates", () =>
{
    var rates = new[]
    {
        new { From = "EUR", To = "USD", Rate = "1.359" },
        new { From = "CAD", To = "EUR", Rate = "0.732" },
        new { From = "USD", To = "EUR", Rate = "0.736" },
        new { From = "EUR", To = "CAD", Rate = "1.366" }
    };

    return rates;
});

app.MapGet("/external-api/v1/transactions", () =>
{
    var transactions = new[]
    {
        new { Sku = "T2006", Amount = "10.00", Currency = "USD" },
        new { Sku = "M2007", Amount = "34.57", Currency = "CAD" },
        new { Sku = "R2008", Amount = "17.95", Currency = "USD" },
        new { Sku = "T2006", Amount = "7.63", Currency = "EUR" },
        new { Sku = "B2009", Amount = "21.23", Currency = "USD" }
    };

    return transactions;
});

app.Run();
