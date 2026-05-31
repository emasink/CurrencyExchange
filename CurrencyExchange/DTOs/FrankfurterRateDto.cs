using System.Text.Json.Serialization;

namespace CurrencyExchange.DTOs;

public class FrankfurterRateDto
{
    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }
}