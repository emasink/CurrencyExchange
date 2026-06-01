using CurrencyExchange.Exceptions;
using CurrencyExchange.Interfaces.Repositories;
using CurrencyExchange.Interfaces.Services;

namespace CurrencyExchange.Services;

public class ExchangeRateService(IExchangeRepository exchangeRepository) : IExchangeRateService
{
    private const string BaseCurrency = "DKK";

    public async Task<decimal> GetExchangeRateAsync(string sourceCurrency, string targetCurrency)
    {
        if (sourceCurrency == targetCurrency)
        {
            return 1;
        }

        var sourceToBaseRate = await GetSourceToBaseCurrencyRate(sourceCurrency);

        if (targetCurrency == BaseCurrency)
        {
            return sourceToBaseRate;
        }

        return await CalculateExchangeRate(targetCurrency, sourceToBaseRate);
    }

    private async Task<decimal> GetSourceToBaseCurrencyRate(string sourceCurrency)
    {
        var rate = await exchangeRepository.GetRateAsync(sourceCurrency);

        return rate ?? throw new RateNotFoundException(sourceCurrency);
    }

    private async Task<decimal> CalculateExchangeRate(string targetCurrency, decimal sourceCurrencyRate)
    {
        var targetToBaseRate = await exchangeRepository.GetRateAsync(targetCurrency);

        return targetToBaseRate is null
            ? throw new RateNotFoundException(targetCurrency)
            : sourceCurrencyRate / targetToBaseRate!.Value;
    }
}