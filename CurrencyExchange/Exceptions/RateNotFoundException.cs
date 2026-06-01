namespace CurrencyExchange.Exceptions;

public class RateNotFoundException(string currency) : Exception($"Rate for {currency} is not available");