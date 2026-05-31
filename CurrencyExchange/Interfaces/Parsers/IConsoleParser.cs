using CurrencyExchange.Requests;

namespace CurrencyExchange.Interfaces.Parsers;

public interface IConsoleParser
{
    ExchangeRequest Parse(string input);
}