//Class responsible for sending emails using SMTP
using System;
using StockQuoteAlert.Models;

namespace StockQuoteAlert.Services
{
    public static class EmailService
    {
        public static void SendAlert(Config config, string symbol, double price, string action)
        {
            Console.WriteLine("=== SIMULAÇÃO DE ENVIO DE EMAIL ===");
            Console.WriteLine($"De: {config.Smtp.Email}");
            Console.WriteLine($"Para: {string.Join(", ", config.Recipients)}");
            Console.WriteLine($"Assunto: Alerta de {action} para {symbol}");
            Console.WriteLine($"Mensagem:");
            Console.WriteLine($"  O preço atual de {symbol} é R${price:F2}.");
            Console.WriteLine($"  Ação recomendada: {action.ToUpper()}");
            Console.WriteLine("====================================");
        }
    }
}