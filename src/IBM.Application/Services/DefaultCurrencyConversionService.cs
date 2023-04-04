using IBM.Application.Contracts.Services;
using IBM.Application.DSA;
using IBM.Application.Models.DTOs.Rates;
using IBM.Application.Models.DTOs.Transactions;
using Microsoft.Extensions.Logging;

namespace IBM.Application.Services;

public class DefaultCurrencyConversionService : ICurrencyConversionService
{
    private readonly ILogger<DefaultCurrencyConversionService> _logger;

    public DefaultCurrencyConversionService(ILogger<DefaultCurrencyConversionService> logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<TransactionDTO>> ApplyRatesToTransactionsAsync(IEnumerable<RateDTO> rates, IEnumerable<TransactionDTO> transactions, string targetCurrency)
    {
        return await Task.Run(() =>
        {
            _logger.LogInformation("Converting transactions to currency '{Currency}'", targetCurrency);

            var graph = GetRatesGraph(rates);

            // Loop only though transactions that are in a different currency than the target currency
            foreach (var transaction in transactions.Where(x => x.Currency != targetCurrency))
            {
                // Find the shortest conversion path between the transaction currency and the target currency
                var startVertex = transaction.Currency;
                var shortestPathFunc = BreadthFirstSearch.ShortestPathFunction(graph, startVertex);
                var shortestPath = shortestPathFunc(targetCurrency);

                _logger.LogInformation("Shortest path to {TargetCurrency}: {ShortestPath}", targetCurrency, string.Join("->", shortestPath));

                // Loop through the steps of the path, skipping the first one because is the current transaction currency
                foreach (var currency in shortestPath.Skip(1))
                {
                    // Apply the rate to the transaction amount and round it
                    transaction.Amount *= rates.First(x => x.From == transaction.Currency && x.To == currency).Rate;
                    transaction.Amount = Math.Round(transaction.Amount, 2, MidpointRounding.ToEven);
                    transaction.Currency = currency;
                }
            }

            return transactions;
        });
    }

    private static Graph<string> GetRatesGraph(IEnumerable<RateDTO> rates)
    {
        // Each currency is a vertex in the graph
        var vertices = rates.Select(x => x.From).Concat(rates.Select(x => x.To)).Distinct().ToArray();

        // Each conversion rate is an edge in the graph
        var edges = new List<Tuple<string, string>>();

        foreach (var rate in rates)
        {
            edges.Add(Tuple.Create(rate.From, rate.To));
        }

        return new Graph<string>(vertices, edges);
    }
}