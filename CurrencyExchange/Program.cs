using CurrencyExchange.Exceptions;
using CurrencyExchange.Extensions;
using CurrencyExchange.Interfaces.Parsers;
using CurrencyExchange.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddServices();
var serviceProvider = services.BuildServiceProvider();

var consoleParser = serviceProvider.GetRequiredService<IConsoleParser>();
var exchangeService = serviceProvider.GetRequiredService<IExchangeService>();

Console.WriteLine("Exchange tool\n" +
                  "Provide currencies and amount in the following format:\n" +
                  "<SourceCurrency>/<TargetCurrency> <Amount>\n" +
                  "\tBoth currencies must be valid ISO literals, i.e. consisting of three capitalised letters\n" +
                  "\tAmount must be a decimal number separated by period.");

while (true)
{
    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input)) continue;
    HandleExit(input);

    try
    {
        await ProcessExchangeRequest(input!);
    }
    catch (StopApplicationException ex)
    {
        Console.WriteLine(ex.Message);
        return;
    }
    catch (Exception exception) when (
        exception is InvalidInputException
            or RateNotFoundException
            or InternalFlowException)
    {
        Console.WriteLine(exception.Message);
    }
    catch (Exception)
    {
        Console.WriteLine("An unknown error occurred.");
    }
}

return;

void HandleExit(string input)
{
    if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
    {
        throw new StopApplicationException("ShuttingDown");
    }
}

async Task ProcessExchangeRequest(string s)
{
    var request = consoleParser.Parse(s);
    var answer = await exchangeService.ExchangeCurrenciesAsync(request);

    Console.WriteLine(answer);
}