using CurrencyExchange.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyExchange.Decorators;

public class CachedExchangeRepository(
    IExchangeRepository exchangeRepository,
    IMemoryCache cache) : IExchangeRepository
{
    public async Task<decimal?> GetRateAsync(string currency)
    {
        var key = $"rate:{currency}";

        if (cache.TryGetValue(key, out decimal? rate))
        {
            return rate;
        }

        rate = await exchangeRepository.GetRateAsync(currency);

        if (rate != null)
        {
            cache.Set(key, rate, TimeSpan.FromMinutes(10));
        }

        return rate;
    }
}