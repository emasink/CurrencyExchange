using CurrencyExchange.Validators;
using FluentAssertions;
using Xunit;

namespace CurrencyExchange.UnitTests;

public class AmountValidatorTests
{
    private readonly AmountValidator _validator = new();

    [Theory]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    [InlineData(1, true)]
    public void Validate(decimal amount, bool expected)
    {
        var actual = _validator.IsValidAmount(amount);
        
        actual.Should().Be(expected);
    }
}