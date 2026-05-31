using CurrencyExchange.Exceptions;
using CurrencyExchange.Interfaces.Services;
using CurrencyExchange.Interfaces.Validators;
using CurrencyExchange.Requests;

namespace CurrencyExchange.Services;

public class ExchangeService(
    ICurrencyValidator currencyValidator,
    IAmountValidator amountValidator,
    IExchangeRateService rateService) 
    : IExchangeService
{
    public async Task<decimal> ExchangeCurrenciesAsync(ExchangeRequest request)
    {
        await ValidateCurrencies(request);
        ValidateAmount(request);
        
        var exchangeRatio = await rateService.GetExchangeRateAsync(request.SourceCurrency, request.TargetCurrency);
        
        return exchangeRatio *  request.Amount;
    }
    
    private async Task ValidateCurrencies(ExchangeRequest request)
    {
        if (! await currencyValidator.IsValidCurrencyAsync(request.SourceCurrency))
        {
            throw new InvalidInputException($"Currency {request.SourceCurrency} is not valid ISO currency literal");
        }

        if (! await currencyValidator.IsValidCurrencyAsync(request.TargetCurrency))
        {
            throw new InvalidInputException($"Currency {request.TargetCurrency} is not valid ISO currency literal");
        }
    }
    
    private void ValidateAmount(ExchangeRequest request)
    {
        if(!amountValidator.IsValidAmount(request.Amount))
            throw new InvalidInputException($"Amount {request.Amount} is invalid, must be positive decimal number");
    }
}