using CurrencyExchange.Exceptions;
using CurrencyExchange.Parsers;
using CurrencyExchange.Requests;
using FluentAssertions;

namespace CurrencyExchange.UnitTests;

public class ConsoleParserTests
{
    private readonly ConsoleParser _parser = new();
    
    [Theory]
    [InlineData("EUR/EUR 1.1", 1.1, "EUR", "EUR")]
    [InlineData("EUR/DKK 10", 10, "EUR", "DKK")]
    public void Parse_WhenValidRequest_ReturnExchangeRequest(string input, decimal amount, string mainCurrencyCode, string moneyCurrencyCode)
    {
        var expected = new ExchangeRequest(amount, mainCurrencyCode, moneyCurrencyCode);
        
        var actual = _parser.Parse(input);

        actual.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData("cur/cur amount", "amount literal")]
    [InlineData("/cur 1", "source currency")]
    [InlineData("cur / 1", "target currency")]
    [InlineData("cur/ 1", "target currency")]
    [InlineData("cur/ cur 1", "target currency")]
    public void Parse_WhenMissingData_ThrowsInvalidInputException(string input, string missingProperty)
    {
        var expectedMessage = $"Couldn't parse {missingProperty}";
        
        _parser
            .Invoking(x => x.Parse(input))
            .Should()
            .ThrowExactly<InvalidInputException>()
            .WithMessage(expectedMessage);;
    }
    
    [Theory]
    [InlineData("cur/cur 79228162514264337593543950336", "amount literal")]
    [InlineData("cur/cur -79228162514264337593543950336", "amount literal")]
    public void Parse_WhenInvalidAmount_ThrowsInvalidInputException(string input, string missingProperty)
    {
        var expectedMessage = $"Couldn't parse {missingProperty}";
        
        _parser
            .Invoking(x => x.Parse(input))
            .Should()
            .ThrowExactly<InvalidInputException>()
            .WithMessage(expectedMessage);;
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  1.1")]
    [InlineData("1.1")]
    [InlineData("abcd")]
    [InlineData("s/s10")]
    [InlineData("EUR/DKK 0 000")]
    [InlineData("EUR-EUR 1")]
    [InlineData("1 d")]
    [InlineData("s 1,")]
    public void Parse_WhenInvalidNumberOfArguments_ThrowsInvalidInputException(string input)
    {
        var expectedMessage = $"Input '{input}' does not follow <CURRENCY PAIR> <AMOUNT> format";
        
        _parser
            .Invoking(x => x.Parse(input))
            .Should()
            .ThrowExactly<InvalidInputException>()
            .WithMessage(expectedMessage);
    }
}