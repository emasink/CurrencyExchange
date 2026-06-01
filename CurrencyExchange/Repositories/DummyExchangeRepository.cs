using CurrencyExchange.DTOs;
using CurrencyExchange.Interfaces.Repositories;

namespace CurrencyExchange.Repositories;

public class DummyExchangeRepository : IExchangeRepository
{
    private const decimal ExchangeTableCoefficient = 100;

    private static readonly Dictionary<string, decimal> ExchangeRates = InitialiseExchangeRates();

    private static Dictionary<string, decimal> InitialiseExchangeRates()
    {
        var rates = new List<ExchangeRateDto>
        {
            new("EUR", 743.94m),
            new("USD", 663.11m),
            new("GBP", 852.85m),
            new("SEK", 76.10m),
            new("NOK", 78.40m),
            new("CHF", 683.58m),
            new("JPY", 5.9740m)
        };

        return rates.ToDictionary(x => x.Currency, x => x.Rate / ExchangeTableCoefficient);
    }

    public async Task<decimal?> GetRateAsync(string currency)
    {
        return ExchangeRates.TryGetValue(currency, out var exchangeRate)
            ? await Task.FromResult(exchangeRate)
            : null;
    }
}