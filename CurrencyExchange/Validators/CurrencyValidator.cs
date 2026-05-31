using CurrencyExchange.Interfaces.Repositories;
using CurrencyExchange.Interfaces.Validators;

namespace CurrencyExchange.Validators;

public class CurrencyValidator(IExchangeRepository exchangeRepository) : ICurrencyValidator
{
    public async Task<bool> IsValidCurrencyAsync(string currencyLiteral)
    {
        return IsBaseCurrency(currencyLiteral) || await IsExchangeRateAvailableForGivenCurrency(currencyLiteral);
    }

    private async Task<bool> IsExchangeRateAvailableForGivenCurrency(string currencyLiteral)
    {
        return await exchangeRepository.GetRateAsync(currencyLiteral) != null;
    }

    private static bool IsBaseCurrency(string currencyLiteral)
    {
        return string.Equals(currencyLiteral, "DKK", StringComparison.OrdinalIgnoreCase);
    }
}