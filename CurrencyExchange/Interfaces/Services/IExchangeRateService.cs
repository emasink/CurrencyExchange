namespace CurrencyExchange.Interfaces;

public interface IExchangeRateService
{
    public Task<decimal> GetExchangeRateAsync(string sourceCurrency, string yargetCurrency);
}