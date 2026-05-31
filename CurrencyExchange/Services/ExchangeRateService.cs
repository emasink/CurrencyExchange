using CurrencyExchange.Interfaces;
using CurrencyExchange.Interfaces.Repositories;

namespace CurrencyExchange.Services;

public class ExchangeRateService(IExchangeRepository exchangeRepository) : IExchangeRateService
{
    private const string BaseCurrency = "DKK";
    public async Task<decimal> GetExchangeRateAsync(string sourceCurrency, string targetCurrency)
    {
        if(sourceCurrency == targetCurrency) return 1;

        var sourceToBaseRate = await exchangeRepository.GetRateAsync(sourceCurrency);
        
        if (targetCurrency == BaseCurrency)
        {
            return sourceToBaseRate!.Value; 
        }

        return await CalculateExchangeRate(targetCurrency, sourceToBaseRate!.Value);
    }

    private async Task<decimal> CalculateExchangeRate(string targetCurrency, decimal sourceCurrencyRate)
    {
        var targetToBaseRate = await exchangeRepository.GetRateAsync(targetCurrency);
        
        return  sourceCurrencyRate / targetToBaseRate!.Value;
    }
}