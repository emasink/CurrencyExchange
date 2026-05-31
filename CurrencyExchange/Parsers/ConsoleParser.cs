using CurrencyExchange.Exceptions;
using CurrencyExchange.Interfaces.Parsers;
using CurrencyExchange.Requests;

namespace CurrencyExchange.Parsers;

public class ConsoleParser : IConsoleParser
{
    public ExchangeRequest Parse(string input)
    {
        var sourceCurrency = GetSourceCurrency(input);
        var targetCurrency = GetTargetCurrency(input);
        
        var amount = GetAmount(input);
        
        return new ExchangeRequest(amount, sourceCurrency, targetCurrency);
    }

    private static string GetSourceCurrency(string input)
    {
        var slashSplitCommand = input.Split("/", StringSplitOptions.TrimEntries);
        if(slashSplitCommand.Length != 2) throw new InvalidInputException($"Input '{input}' does not follow <CURRENCY PAIR> <AMOUNT> format");
        
        var sourceCurrency = slashSplitCommand[0];
        
        return string.IsNullOrEmpty(sourceCurrency)
            ? throw new InvalidInputException("Couldn't parse source currency")
            : sourceCurrency;
    }
    
    private static string GetTargetCurrency(string input)
    {
        try
        {
            var targetCurrency =  input
                .Split("/")[1]
                .Split(" ")[0];
            
            return string.IsNullOrEmpty(targetCurrency)
                ? throw new InvalidInputException("Couldn't parse source currency")
                : targetCurrency;
        }
        catch (Exception )
        {
            throw new InvalidInputException("Couldn't parse target currency");
        }
    }
    
    private static decimal GetAmount(string input)
    {
        try
        {
            var currencyAndAmountList = input
                .Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1]
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (currencyAndAmountList.Length != 2)
            {
                throw new InvalidInputException($"Input '{input}' does not follow <CURRENCY PAIR> <AMOUNT> format");
            }
            
            var amountLiteral = currencyAndAmountList[1];

            return decimal.Parse(amountLiteral);
        }
        catch (InvalidInputException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new InvalidInputException("Couldn't parse amount literal");
        }
    }
}