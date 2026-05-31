using CurrencyExchange.Interfaces.Validators;

namespace CurrencyExchange.Validators;

public class AmountValidator : IAmountValidator
{
    public bool IsValidAmount(decimal amount)
    {
        return amount > 0;
    }
}