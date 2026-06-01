using CurrencyExchange.Interfaces.Validators;

namespace CurrencyExchange.Validators;

public class ExchangeRequestValidator : IExchangeRequestValidator
{
    public bool IsValidCurrencyFormat(string currencyLiteral)
    {
        return currencyLiteral.All(char.IsAsciiLetterUpper)
               && currencyLiteral.Length == 3;
    }

    public bool IsAmountPositive(decimal amount)
    {
        return amount > 0;
    }
}