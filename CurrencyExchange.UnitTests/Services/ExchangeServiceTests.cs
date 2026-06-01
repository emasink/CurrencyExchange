using CurrencyExchange.Exceptions;
using CurrencyExchange.Interfaces.Services;
using CurrencyExchange.Interfaces.Validators;
using CurrencyExchange.Requests;
using CurrencyExchange.Services;
using FluentAssertions;
using NSubstitute;

namespace CurrencyExchange.UnitTests.Services;

public class ExchangeServiceTests
{
    private const decimal Amount = 12;
    private const string SourceCurrency = "CUR1";
    private const string TargetCurrency = "CUR2";
    private const decimal Ratio = 2;

    private readonly IExchangeRateService _exchangeRateService = Substitute.For<IExchangeRateService>();
    private readonly IExchangeRequestValidator _exchangeRequestValidator = Substitute.For<IExchangeRequestValidator>();

    private readonly ExchangeService _service;

    public ExchangeServiceTests()
    {
        _service = new ExchangeService(_exchangeRequestValidator, _exchangeRateService);
    }

    [Fact]
    public async Task ExchangeCurrencies_ValidCurrencies_ReturnProductOfRatioAndAmount()
    {
        _exchangeRequestValidator.IsValidCurrencyFormat(SourceCurrency).Returns(true);
        _exchangeRequestValidator.IsValidCurrencyFormat(TargetCurrency).Returns(true);
        _exchangeRequestValidator.IsAmountPositive(Amount).Returns(true);
        _exchangeRateService.GetExchangeRateAsync(SourceCurrency, TargetCurrency).Returns(Ratio);

        var exchangeRequest = new ExchangeRequest(Amount, SourceCurrency, TargetCurrency);

        var actual = await _service.ExchangeCurrenciesAsync(exchangeRequest);

        actual.Should().Be(Amount * Ratio);
    }

    [Fact]
    public async Task ExchangeCurrencies_InvalidSourceCurrency_ThrowInvalidInputException()
    {
        var exchangeRequest = new ExchangeRequest(Amount, SourceCurrency, TargetCurrency);
        _exchangeRequestValidator.IsValidCurrencyFormat(SourceCurrency).Returns(false);

        await _service.Invoking(x => x.ExchangeCurrenciesAsync(exchangeRequest))
            .Should()
            .ThrowExactlyAsync<InvalidCurrencyException>()
            .WithMessage($"{SourceCurrency} is not valid ISO currency literal");

        _exchangeRequestValidator.Received(0).IsValidCurrencyFormat(TargetCurrency);
        _exchangeRequestValidator.Received(0).IsAmountPositive(Amount);
        await _exchangeRateService.Received(0).GetExchangeRateAsync(SourceCurrency, TargetCurrency);
    }

    [Fact]
    public async Task ExchangeCurrencies_InvalidTargetCurrency_ThrowInvalidInputException()
    {
        _exchangeRequestValidator.IsValidCurrencyFormat(SourceCurrency).Returns(true);
        _exchangeRequestValidator.IsValidCurrencyFormat(TargetCurrency).Returns(false);

        await _service.Invoking(x =>
                x.ExchangeCurrenciesAsync(new ExchangeRequest(Amount, SourceCurrency, TargetCurrency)))
            .Should()
            .ThrowExactlyAsync<InvalidCurrencyException>()
            .WithMessage($"{TargetCurrency} is not valid ISO currency literal");

        await _exchangeRateService.Received(0).GetExchangeRateAsync(SourceCurrency, TargetCurrency);
        _exchangeRequestValidator.Received(0).IsAmountPositive(Amount);
    }

    [Fact]
    public async Task ExchangeCurrencies_InvalidAmount_ThrowInvalidInputException()
    {
        _exchangeRequestValidator.IsValidCurrencyFormat(SourceCurrency).Returns(true);
        _exchangeRequestValidator.IsValidCurrencyFormat(TargetCurrency).Returns(true);

        await _service.Invoking(x =>
                x.ExchangeCurrenciesAsync(new ExchangeRequest(-1, SourceCurrency, TargetCurrency)))
            .Should()
            .ThrowExactlyAsync<InvalidAmountException>()
            .WithMessage($"Amount must be positive");

        await _exchangeRateService.Received(0).GetExchangeRateAsync(SourceCurrency, TargetCurrency);
    }
}