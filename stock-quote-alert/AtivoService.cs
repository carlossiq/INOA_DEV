using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace StockQuoteAlert.Services
{
    public static class AtivoService
    {
        private static readonly HttpClient httpClient = new();

        public static async Task<double?> GetCurrentPrice(string symbol, string token)
        {
            if (string.IsNullOrWhiteSpace(symbol) || string.IsNullOrWhiteSpace(token))
                return null;

            string url = $"https://brapi.dev/api/quote/{symbol}?token={token}&range=1d&interval=1d";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    await Task.Delay(5 * 60 * 1000);
                    return null;
                }

                string json = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("results", out var results) && results.GetArrayLength() > 0)
                {
                    var asset = results[0];

                    if (asset.TryGetProperty("regularMarketPrice", out var priceElement))
                    {
                        return priceElement.GetDouble();
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return null;
            }
        }
    }
}
