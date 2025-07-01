using System;
using System.Globalization;
using System.Threading.Tasks;
using StockQuoteAlert.Models;
using StockQuoteAlert.Services;
using StockQuoteAlert.Utils;

namespace StockQuoteAlert
{
    class Program
    {
        /// Using Task for it is asynchronous and can be extended in the future
        static async Task Main(string[] args)
        {
            // Loading configuration
            Config config;
            try
            {
                config = ConfigLoader.Load();
                Language.Set(config.Language);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{Language.Get("LoadingConfigError")} {ex.Message}");
                return;
            }
            
            // Check if the correct number of arguments is provided
            if (args.Length != 3)
            {
                Console.WriteLine(Language.Get("Usage"));
                return;
            }

            string symbol = args[0].ToUpper();

            // Validate and parse the sell and buy prices in American format(used by the API)
            if (!double.TryParse(args[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double sellPrice))
            {
                Console.WriteLine(Language.Get("InvalidSellPrice"));
                return;
            }

            if (!double.TryParse(args[2], NumberStyles.Any, CultureInfo.InvariantCulture, out double buyPrice))
            {
                Console.WriteLine(Language.Get("InvalidBuyPrice"));
                return;
            }

            if (string.IsNullOrEmpty(config.TwelveDataApiKey))
            {
                Console.WriteLine(Language.Get("MissingApiKey"));
                return;
            }

            double? currentPrice = await AtivoService.GetCurrentPrice(symbol, config.TwelveDataApiKey);
            if (currentPrice == null)
            {
                Console.WriteLine(Language.Get("FailedToGetPrice"));
                return;
            }

            Console.WriteLine(Language.Get("CurrentPrice", symbol, currentPrice.Value.ToString("F2", CultureInfo.InvariantCulture)));

            // Logic of recommendation
            if (currentPrice > sellPrice)
            {
                Console.WriteLine(Language.Get("PriceAbove"));
                EmailService.SendAlert(config, symbol, currentPrice.Value, Language.Get("PriceAbove"));
            }
            else if (currentPrice < buyPrice)
            {
                Console.WriteLine(Language.Get("PriceBelow"));
                EmailService.SendAlert(config, symbol, currentPrice.Value, Language.Get("PriceBelow"));
            }
            else
            {
                Console.WriteLine(Language.Get("PriceHold"));
            }
        }
    }
}
