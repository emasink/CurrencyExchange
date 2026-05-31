using System.Net;
using System.Net.Http.Json;
using CurrencyExchange.DTOs;
using CurrencyExchange.Exceptions;
using CurrencyExchange.Interfaces.Repositories;

namespace CurrencyExchange.Repositories;

public class ExchangeRepository(HttpClient httpClient) : IExchangeRepository
{
    public async Task<decimal?> GetRateAsync(string toCurrency)
    {
        try
        {
            var result = await httpClient.GetAsync($"rates?base={toCurrency}&quotes=DKK");

            if (IsUnprocessableEntity(result))
            {
                return null;
            }
        
            result.EnsureSuccessStatusCode();
            
            var rates = await result.Content.ReadFromJsonAsync<FrankfurterRateDto[]>();
    
            return rates?.Length > 0 ? rates[0].Rate : null;
        }
        catch (HttpRequestException)
        {
            throw new InternalFlowException("Request to currency exchange failed");
        }
        
    }

    private static bool IsUnprocessableEntity(HttpResponseMessage result)
    {
        return result.StatusCode == HttpStatusCode.UnprocessableEntity;
    }
}