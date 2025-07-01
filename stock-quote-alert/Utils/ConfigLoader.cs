using System;
using System.IO;
using System.Text.Json;
using System.Net.Mail;
using StockQuoteAlert.Models;
using StockQuoteAlert.Utils;

namespace StockQuoteAlert.Utils
{
    public static class ConfigLoader
    {
        public static Config Load(string path = "config.json")
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Configuration file not found: {path}");

            string configJson = File.ReadAllText(path);

            Config config = JsonSerializer.Deserialize<Config>(configJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (config == null)
                throw new Exception("Failed to deserialize the configuration file.");

            Validate(config);

            string lang = config.Language ?? "en";
            if (!Language.IsSupported(lang))
                lang = "en";

            Language.Set(lang);

            return config;
        }

        private static void Validate(Config config)
        {
            if (config.Smtp == null)
                throw new Exception("SMTP configuration is missing.");

            if (string.IsNullOrWhiteSpace(config.Smtp.Host))
                throw new Exception("SMTP host is not specified.");

            if (config.Smtp.Port == 0)
                throw new Exception("SMTP port is invalid.");

            if (string.IsNullOrWhiteSpace(config.Smtp.Email))
                throw new Exception("SMTP email is not specified.");

            if (!IsValidEmail(config.Smtp.Email))
                throw new Exception($"Invalid SMTP email: {config.Smtp.Email}");

            if (string.IsNullOrWhiteSpace(config.Smtp.Password))
                throw new Exception("SMTP password is not specified.");

            if (config.Recipients == null || config.Recipients.Count == 0)
                throw new Exception("No recipients configured.");

            foreach (var recipient in config.Recipients)
            {
                if (!IsValidEmail(recipient))
                    throw new Exception($"Invalid recipient email: {recipient}");
            }
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
