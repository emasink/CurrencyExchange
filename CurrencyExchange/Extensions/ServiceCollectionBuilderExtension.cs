using CurrencyExchange.Decorators;
using CurrencyExchange.Interfaces.Parsers;
using CurrencyExchange.Interfaces.Repositories;
using CurrencyExchange.Interfaces.Services;
using CurrencyExchange.Interfaces.Validators;
using CurrencyExchange.Parsers;
using CurrencyExchange.Repositories;
using CurrencyExchange.Services;
using CurrencyExchange.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyExchange.Extensions;

public static class ServiceCollectionBuilderExtension
{
    public static void AddServices(this ServiceCollection services)
    {
        services.AddMemoryCache();
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

        services.Decorate<IExchangeRepository, CachedExchangeRepository>();
    }
}