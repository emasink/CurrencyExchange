using System.Threading.Tasks;
using CurrencyExchange.Interfaces.Repositories;
using CurrencyExchange.Validators;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CurrencyExchange.UnitTests;

public class CurrencyValidatorTests
{
    private const string ValidCurrencyLiteral = "EUR";
    private const string BaseCurrencyLiteral = "DKK";
    private const string InvalidCurrencyLiteral = "CUR";
    private const decimal Rate = 1m;

    private readonly IExchangeRepository _exchangeRepository = Substitute.For<IExchangeRepository>();
    
    private readonly CurrencyValidator _currencyValidator;

    public CurrencyValidatorTests()
    {
        _currencyValidator = new CurrencyValidator(_exchangeRepository);
    }

    [Fact]
    public async Task IsValid_WhenRepositoryReturnsRate_ReturnTrue()
    {
        _exchangeRepository
            .GetRateAsync(ValidCurrencyLiteral)
            .Returns(Rate);
            
        var actual = await _currencyValidator.IsValidCurrencyAsync(ValidCurrencyLiteral);
        
        actual.Should().BeTrue();
    }

    [Fact]
    public async Task IsValid_WhenRepositoryReturnsNull_ReturnTrue()
    {
        _exchangeRepository
            .GetRateAsync(InvalidCurrencyLiteral)
            .Returns((decimal?)null);
            
        var actual = await _currencyValidator.IsValidCurrencyAsync(InvalidCurrencyLiteral);
        
        actual.Should().BeFalse();
    }
    
    [Fact]
    public async Task IsValid_WhenBaseCurrencyLiteral_ReturnTrue()
    {
        var actual = await _currencyValidator.IsValidCurrencyAsync(BaseCurrencyLiteral);

        actual
            .Should()
            .BeTrue();
        
        await _exchangeRepository
            .Received(0)
            .GetRateAsync(BaseCurrencyLiteral);
    }
}