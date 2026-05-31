using System.Threading.Tasks;
using CurrencyExchange.Exceptions;
using CurrencyExchange.Interfaces;
using CurrencyExchange.Interfaces.Validators;
using CurrencyExchange.Requests;
using CurrencyExchange.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CurrencyExchange.UnitTests;

public class ExchangeServiceTests
{
    private const decimal Amount = 12;
    private const string SourceCurrency = "CUR1";
    private const string TargetCurrency = "CUR2";
    private const decimal Ratio = 2;

    private readonly IExchangeRateService _exchangeRateService = Substitute.For<IExchangeRateService>();
    private readonly ICurrencyValidator _currencyValidator = Substitute.For<ICurrencyValidator>();
    private readonly IAmountValidator _amountValidator = Substitute.For<IAmountValidator>();

    private readonly ExchangeService _service;

    public ExchangeServiceTests()
    {
        _service = new ExchangeService(_currencyValidator, _amountValidator, _exchangeRateService);
    }
    
    [Fact]
    public async Task ExchangeCurrencies_ValidCurrencies_ReturnProductOfRatioAndAmount()
    {
        _currencyValidator.IsValidCurrencyAsync(SourceCurrency).Returns(true);
        _currencyValidator.IsValidCurrencyAsync(TargetCurrency).Returns(true);
        _amountValidator.IsValidAmount(Amount).Returns(true);
        _exchangeRateService.GetExchangeRateAsync(SourceCurrency, TargetCurrency).Returns(Ratio);
        
        var exchangeRequest = new ExchangeRequest(Amount, SourceCurrency,  TargetCurrency);

        var actual = await _service.ExchangeCurrenciesAsync(exchangeRequest);

        actual.Should().Be(Amount * Ratio);
    }
    
    [Fact]
    public async Task ExchangeCurrencies_InvalidAmount_ReturnProductOfRatioAndAmount()
    {
        _currencyValidator.IsValidCurrencyAsync(SourceCurrency).Returns(true);
        _currencyValidator.IsValidCurrencyAsync(TargetCurrency).Returns(true);
        _amountValidator.IsValidAmount(Amount).Returns(false);
        _exchangeRateService.GetExchangeRateAsync(SourceCurrency, TargetCurrency).Returns(Ratio);
        
        await _service.Invoking(x => x.ExchangeCurrenciesAsync(new ExchangeRequest(Amount, SourceCurrency, TargetCurrency)))
            .Should()
            .ThrowAsync<InvalidInputException>()
            .WithMessage($"Amount {Amount} is invalid, must be positive decimal number");
    }

    [Fact]
    public void ExchangeCurrencies_InvalidSourceCurrency_ThrowInvalidInputException()
    {
        _currencyValidator.IsValidCurrencyAsync(SourceCurrency).Returns(false);
        
        _service.Invoking(x => x.ExchangeCurrenciesAsync(new ExchangeRequest(Amount, SourceCurrency, TargetCurrency)))
            .Should()
            .ThrowAsync<InvalidInputException>()
            .WithMessage($"Currency {SourceCurrency} is not valid ISO currency literal");
        
        _currencyValidator.Received(0).IsValidCurrencyAsync(TargetCurrency);
        _exchangeRateService.Received(0).GetExchangeRateAsync(SourceCurrency, TargetCurrency);
    }
    
    [Fact]
    public void ExchangeCurrencies_InvalidTargetCurrency_ThrowInvalidInputException()
    {
        _currencyValidator.IsValidCurrencyAsync(SourceCurrency).Returns(true);
        _currencyValidator.IsValidCurrencyAsync(TargetCurrency).Returns(false);
        
        _service.Invoking(x => x.ExchangeCurrenciesAsync(new ExchangeRequest(Amount, SourceCurrency, TargetCurrency)))
            .Should()
            .ThrowAsync<InvalidInputException>()
            .WithMessage($"Currency {TargetCurrency} is not valid ISO currency literal");
        
        _exchangeRateService.Received(0).GetExchangeRateAsync(SourceCurrency, TargetCurrency);
    }
}