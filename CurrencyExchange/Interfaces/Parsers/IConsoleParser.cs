using CurrencyExchange.Requests;

namespace CurrencyExchange.Interfaces;

public interface IConsoleParser
{
    ExchangeRequest Parse(string input);
}