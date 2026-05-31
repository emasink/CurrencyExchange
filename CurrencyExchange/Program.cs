using CurrencyExchange;
using CurrencyExchange.Exceptions;
using CurrencyExchange.Interfaces;
using CurrencyExchange.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddServices();



Console.WriteLine("Exchange tool\n" +
                  "Provide currencies and amount in the following format:\n" +
                  "<SourceCurrency>/<TargetCurrency> <Amount>\n" +
                  "\tBoth currencies must be valid ISO literals, i.e. consisting of three capitalised letters\n" +
                  "\tAmount must be a decimal number separated by period.");

while (true)
{
    var input = Console.ReadLine();
    
    try
    {
        HandleExit(input);

        await ProcessExchangeRequest(input);
    }
    catch (StopApplicationException ex)
    {
        Console.WriteLine(ex.Message);
        return;
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
void HandleExit(string input)
{
    if (input.Equals("EXIT", StringComparison.InvariantCultureIgnoreCase))
    {
        throw new StopApplicationException("ShuttingDown");
    }
}

async Task ProcessExchangeRequest( string s)
{
    var serviceProvider = services.BuildServiceProvider();

    var consoleParser = serviceProvider.GetService<IConsoleParser>();
    var exchangeService = serviceProvider.GetService<IExchangeService>();
    
    var request = consoleParser!.Parse(s!);
    var answer = await exchangeService!.ExchangeCurrenciesAsync(request);
    
    Console.WriteLine(answer);
}
