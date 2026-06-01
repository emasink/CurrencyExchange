namespace CurrencyExchange.Interfaces.Validators;

public interface IExchangeRequestValidator
{
    bool IsValidCurrencyFormat(string currencyLiteral);
    bool IsAmountPositive(decimal amount);
}