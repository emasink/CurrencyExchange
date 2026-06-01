using CurrencyExchange.Exceptions;
using CurrencyExchange.Interfaces.Services;
using CurrencyExchange.Interfaces.Validators;
using CurrencyExchange.Requests;

namespace CurrencyExchange.Services;

public class ExchangeService(
    IExchangeRequestValidator exchangeRequestValidator,
    IExchangeRateService rateService)
    : IExchangeService
{
    public async Task<decimal> ExchangeCurrenciesAsync(ExchangeRequest request)
    {
        ValidateRequest(request);

        var exchangeRatio = await rateService.GetExchangeRateAsync(request.SourceCurrency, request.TargetCurrency);

        return exchangeRatio * request.Amount;
    }

    private void ValidateRequest(ExchangeRequest request)
    {
        if (!exchangeRequestValidator.IsValidCurrencyFormat(request.SourceCurrency))
        {
            throw new InvalidCurrencyException(request.SourceCurrency);
        }

        if (!exchangeRequestValidator.IsValidCurrencyFormat(request.TargetCurrency))
        {
            throw new InvalidCurrencyException(request.TargetCurrency);
        }

        if (0 >= request.Amount)
        {
            throw new InvalidAmountException("Amount must be positive");
        }
    }
}