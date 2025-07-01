using System.IO;
using System.Globalization;
using StockQuoteAlert.Utils;

namespace StockQuoteAlert.Services
{
    public static class EmailTemplate
    {
        public static string LoadTemplate(string symbol, string price, string action)
        {
            string templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "EmailTemplate.html");
            string template = File.ReadAllText(templatePath);

            string lang = Language.GetCurrentLanguage();
            string currentPriceText = Language.Get("CurrentPrice", symbol, price);
            string subject = Language.Get("EmailAlertSubject", symbol);
            string footer = Language.Get("EmailFooter");
            string footer2 = Language.Get("EmailFooter2");

            return template
                .Replace("{{lang}}", lang)
                .Replace("{{symbol}}", symbol)
                .Replace("{{CurrentPrice}}", currentPriceText)
                .Replace("{{EmailAlertSubject}}", subject)
                .Replace("{{ActionRecommendation}}", action)
                .Replace("{{EmailFooter}}", footer)
                .Replace("{{EmailFooter2}}", footer2);
        }
    }
}
