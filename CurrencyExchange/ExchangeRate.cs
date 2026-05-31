namespace CurrencyExchange;

public class ExchangeRate(string currency, decimal rate)
{
    public string Currency { get; set; } = currency;
    public decimal Rate { get; set; } = rate;
}