namespace CurrencyExchange.Requests;

public record ExchangeRequest (decimal Amount, string SourceCurrency, string TargetCurrency);