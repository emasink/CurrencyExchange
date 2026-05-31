namespace CurrencyExchange.Interfaces.Repositories;

public interface IExchangeRepository
{
    public Task<decimal?> GetRateAsync(string toCurrency);
}