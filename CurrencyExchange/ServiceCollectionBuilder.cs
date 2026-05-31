using CurrencyExchange.Interfaces;
using CurrencyExchange.Interfaces.Repositories;
using CurrencyExchange.Interfaces.Services;
using CurrencyExchange.Interfaces.Validators;
using CurrencyExchange.Repositories;
using CurrencyExchange.Services;
using CurrencyExchange.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyExchange;

public static class ServiceCollectionBuilder
{
    public static void AddServices(this ServiceCollection services)
    {
        services.AddScoped<IConsoleParser, ConsoleParser>();
        services.AddScoped<IExchangeService, ExchangeService>();
        services.AddScoped<ICurrencyValidator, CurrencyValidator>();
        services.AddScoped<IAmountValidator, AmountValidator>();
        services.AddScoped<IExchangeRateService, ExchangeRateService>();

        // services.AddScoped<IExchangeRepository, DummyExchangeRepository>();
        services.AddHttpClient<IExchangeRepository, ExchangeRepository>(client =>
        {
            client.BaseAddress = new Uri("https://api.frankfurter.dev/v2/");
        });
    }
    
}