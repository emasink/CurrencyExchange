namespace CurrencyExchange.Interfaces.Services;

public interface IExchangeRateService
{
    public Task<decimal> GetExchangeRateAsync(string sourceCurrency, string yargetCurrency);
}