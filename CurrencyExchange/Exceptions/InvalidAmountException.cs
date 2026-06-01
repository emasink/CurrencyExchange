namespace CurrencyExchange.Exceptions;

public class InvalidAmountException(string message) : InvalidInputException(message);