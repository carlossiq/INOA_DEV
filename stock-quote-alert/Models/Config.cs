// Define the format for json file
using System.Collections.Generic;

namespace StockQuoteAlert.Models
{
    public class Config
    {
        public SmtpConfig? Smtp { get; set; }
        public List<string>? Recipients { get; set; }
        public string? Language { get; set; }
        public string? TwelveDataApiKey { get; set; }
    }

    public class SmtpConfig
    {
        public string? Host { get; set; }
        public int Port { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}

