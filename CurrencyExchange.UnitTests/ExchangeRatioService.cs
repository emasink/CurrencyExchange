using System.Threading.Tasks;
using CurrencyExchange.Interfaces.Repositories;
using CurrencyExchange.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CurrencyExchange.UnitTests;

public class ExchangeRateServiceTests
{
    private const string BaseCurrency = "DKK";
    private const string Currency1 = "EUR";
    private const string Currency2 = "USD";
    private const decimal Rate1 = 2m;
    private const decimal Rate2 = 0.5m;

    private readonly IExchangeRepository _exchangeRepository = Substitute.For<IExchangeRepository>();
    
    private readonly ExchangeRateService _service;

    public ExchangeRateServiceTests()
    {
        _service = new ExchangeRateService(_exchangeRepository);
    }
    
    [Fact]
    public async Task GetRatio_WhenSameSourceAndTargetCurrencies_Return1()
    {
        var actual = await _service.GetExchangeRateAsync(Currency1, Currency1);
        
        actual.Should().Be(1);
    }

    [Fact]
    public async Task GetRatio_WhenTargetCurrencyIsBaseCurrency_ReturnFromRepository()
    {
        _exchangeRepository.GetRateAsync(Currency1)
            .Returns(Rate1);
        
        var actual = await _service.GetExchangeRateAsync(Currency1, BaseCurrency);

        actual.Should().Be(Rate1);
    }

    [Fact]
    public async Task GetRatio_WhenTargetCurrencyIsNotBaseCurrency_CalculateRatio()
    {
        const int expected = 4;
        _exchangeRepository.GetRateAsync(Currency1)
            .Returns(Rate1);
        
        _exchangeRepository.GetRateAsync(Currency2)
            .Returns(Rate2);
        
        var actual = await _service.GetExchangeRateAsync(Currency1, Currency2);

        actual.Should().Be(expected);
    }
}