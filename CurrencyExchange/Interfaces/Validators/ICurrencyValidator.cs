namespace CurrencyExchange.Interfaces.Validators;

public interface ICurrencyValidator
{
    Task<bool> IsValidCurrencyAsync(string currencyLiteral);
}