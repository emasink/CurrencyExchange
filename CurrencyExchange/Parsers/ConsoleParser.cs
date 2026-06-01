using CurrencyExchange.Exceptions;
using CurrencyExchange.Interfaces.Parsers;
using CurrencyExchange.Requests;

namespace CurrencyExchange.Parsers;

public class ConsoleParser : IConsoleParser
{
    private const string InvalidFormatMessage = $"Input does not follow <CURRENCY PAIR> <AMOUNT> format";

    public ExchangeRequest Parse(string input)
    {
        var (sourceCurrency, targetCurrencyWithAmount) = GetSourceCurrency(input);
        var (targetCurrency, amountLiteral) = GetTargetCurrency(targetCurrencyWithAmount);

        var amount = GetAmount(amountLiteral);

        return new ExchangeRequest(amount, sourceCurrency, targetCurrency);
    }

    private static (string, string) GetSourceCurrency(string input)
    {
        var slashSplitCommand = input.Split("/");
        if (slashSplitCommand.Length != 2)
            throw new InvalidInputException(InvalidFormatMessage);

        var sourceCurrency = slashSplitCommand[0];

        return string.IsNullOrEmpty(sourceCurrency) || sourceCurrency.Any(char.IsWhiteSpace)
            ? throw new InvalidInputException("Couldn't parse source currency")
            : (sourceCurrency, slashSplitCommand[1]);
    }

    private static (string, string) GetTargetCurrency(string targetCurrencyWithAmount)
    {
        var targetCurrencyAndAmount = targetCurrencyWithAmount.Split(" ");

        if (targetCurrencyAndAmount.Length != 2)
        {
            throw new InvalidInputException(InvalidFormatMessage);
        }

        var targetCurrency = targetCurrencyAndAmount[0];
        return string.IsNullOrEmpty(targetCurrency) || targetCurrency.Any(char.IsWhiteSpace)
            ? throw new InvalidInputException("Couldn't parse target currency")
            : (targetCurrency, targetCurrencyAndAmount[1]);
    }

    private static decimal GetAmount(string amountLiteral)
    {
        try
        {
            return decimal.Parse(amountLiteral);
        }
        catch (Exception)
        {
            throw new InvalidInputException("Couldn't parse amount literal");
        }
    }
}