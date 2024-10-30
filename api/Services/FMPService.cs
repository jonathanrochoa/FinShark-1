using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Newtonsoft.Json;

namespace api.Services
{
    public class FMPService : IFMPService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public FMPService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<Stock> FindStockBySymbolAsync(string symbol)
        {
            try
            {
                Console.WriteLine($"Starting to fetch stock data for symbol: {symbol}");

                var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_configuration["FMPKey"]}");

                if(result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    Console.WriteLine("API call succeeded. Response content:");
                    Console.WriteLine(content);

                    var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
                    var fmpStock = tasks.FirstOrDefault();
                    if(fmpStock != null)
                    {
                        Console.WriteLine($"Deserialized FMPStock data: Symbol = {fmpStock.symbol}, CompanyName = {fmpStock.companyName}, Price = {fmpStock.price}");
                        var stock = fmpStock.ToStockFromFMP();
                        return stock;
                    }
                    else
                    {
                        Console.WriteLine("No stock data found for this symbol in API response.");
                    }
                }
                else
                {
                    Console.WriteLine($"API call failed with status code: {result.StatusCode}");
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}