using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace StockQuoteAlert.Services
{
    public static class AtivoService
    {
        private static readonly HttpClient httpClient = new();

        public static async Task<double?> GetCurrentPrice(string symbol, string apiKey)
        {
            if (string.IsNullOrWhiteSpace(symbol) || string.IsNullOrWhiteSpace(apiKey))
                return null;

            string url = $"https://api.twelvedata.com/price?symbol={symbol}&apikey={apiKey}";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                using JsonDocument doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("price", out var priceElement))
                {
                    string? priceString = priceElement.GetString();
                    if (priceString != null && double.TryParse(priceString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double price))
                    {
                        return price;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao consultar API da Twelve Data: {ex.Message}");
                return null;
            }
        }
    }
}