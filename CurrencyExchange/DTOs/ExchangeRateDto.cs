namespace CurrencyExchange.DTOs;

public class ExchangeRateDto(string currency, decimal rate)
{
    public string Currency { get; } = currency;
    public decimal Rate { get; } = rate;
}