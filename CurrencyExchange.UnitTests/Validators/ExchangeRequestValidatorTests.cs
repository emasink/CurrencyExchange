using CurrencyExchange.Validators;
using FluentAssertions;

namespace CurrencyExchange.UnitTests.Validators;

public class ExchangeRequestValidatorTests
{
    private readonly ExchangeRequestValidator _validator;

    public ExchangeRequestValidatorTests()
    {
        _validator = new ExchangeRequestValidator();
    }


    [Theory]
    [InlineData("EUR")]
    [InlineData("AAA")]
    public void IsValidCurrencyFormat_WhenValidIsoCurrencyLiteral_ReturnTrue(string currencyLiteral)
    {
        var actual = _validator.IsValidCurrencyFormat(currencyLiteral);

        actual.Should().BeTrue();
    }

    [Theory]
    [InlineData("EU")]
    [InlineData("")]
    [InlineData("EURO")]
    [InlineData("eur")]
    public void IsValidCurrencyFormat_WhenInvalidCurrencyLiteral_ReturnFalse(string currencyLiteral)
    {
        var actual = _validator.IsValidCurrencyFormat(currencyLiteral);

        actual.Should().BeFalse();
    }
}