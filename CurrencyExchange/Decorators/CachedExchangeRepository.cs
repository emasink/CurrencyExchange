using CurrencyExchange.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyExchange.Decorators;

public class CachedExchangeRepository(
    IExchangeRepository exchangeRepository,
    IMemoryCache cache) : IExchangeRepository
{
    public async Task<decimal?> GetRateAsync(string toCurrency)
    {
        var key = $"rate:{toCurrency}";

        if (cache.TryGetValue(key, out decimal? rate))
        {
            return rate;
        }

        rate = await exchangeRepository.GetRateAsync(toCurrency);

        cache.Set(key, rate, TimeSpan.FromMinutes(10));

        return rate;
    }
}