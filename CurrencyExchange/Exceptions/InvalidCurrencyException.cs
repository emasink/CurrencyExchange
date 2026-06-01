namespace CurrencyExchange.Exceptions;

public class InvalidCurrencyException(string currency)
    : InvalidInputException($"{currency} is not valid ISO currency literal");