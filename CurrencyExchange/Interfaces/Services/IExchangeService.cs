using CurrencyExchange.Requests;

namespace CurrencyExchange.Interfaces.Services;

public interface IExchangeService
{
    Task<decimal> ExchangeCurrenciesAsync(ExchangeRequest request);
}