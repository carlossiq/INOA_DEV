//Class responsible for monitoring stock quotes
namespace StockQuoteAlert.Services
{
    public static class AtivoService
    {
        public static Task<double?> GetCurrentPrice(string symbol, string apiKey)
        {
            // Simulando uma resposta da API
            return Task.FromResult<double?>(38.70);
        }
    }
}