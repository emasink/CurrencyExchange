namespace CurrencyExchange.Interfaces.Validators;

public interface IAmountValidator
{
    bool IsValidAmount(decimal amount);
}