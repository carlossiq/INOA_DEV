//Class responsible for sending emails using SMTP
using System;
using System.Net;
using System.Net.Mail;
using System.Globalization;
using StockQuoteAlert.Models;
using StockQuoteAlert.Utils;

namespace StockQuoteAlert.Services
{
    public static class EmailService
    {
        public static void SendAlert(Config config, string symbol, double price, string action)
        {
            if (config.Smtp == null || string.IsNullOrWhiteSpace(config.Smtp.Email) || string.IsNullOrWhiteSpace(config.Smtp.Password))
            {
                Console.WriteLine(Language.Get("InvalidSmtpConfig"));
                return;
            }

            var smtpClient = new SmtpClient(config.Smtp.Host, config.Smtp.Port)
            {
                Credentials = new NetworkCredential(config.Smtp.Email, config.Smtp.Password),
                EnableSsl = true
            };

            string subject = Language.Get("EmailAlertSubject", action, symbol);
            string priceStr = price.ToString("F2", CultureInfo.InvariantCulture);
            string color = action.ToUpper() == "BUY" ? "green" : "red";
            string date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            string bodyHtml = $@"
        <html>
          <body style='font-family: Arial, sans-serif; line-height: 1.5;'>
            <h2 style='color: #007bff;'>📈 {Language.Get("EmailAlertSubject", action, symbol)}</h2>
            <p>
              <strong>{Language.Get("CurrentPrice", symbol, priceStr)}</strong><br>
              <strong>{Language.Get("ActionRecommendation", $"<span style='color:{color}'>{action.ToUpper()}</span>")}</strong><br>
              <strong>Data:</strong> {date}
            </p>
            <hr>
            <p style='font-size: 0.9em; color: #666;'>
              {Language.Get("EmailFooter")}
            </p>
          </body>
        </html>";

            try
            {
                foreach (var recipient in config.Recipients ?? new List<string>())
                {
                    var mail = new MailMessage
                    {
                        From = new MailAddress(config.Smtp.Email),
                        Subject = subject,
                        Body = bodyHtml,
                        IsBodyHtml = true
                    };

                    mail.To.Add(recipient);
                    smtpClient.Send(mail);
                }

                Console.WriteLine(Language.Get("EmailSentSuccess"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(Language.Get("EmailSendError") + $" {ex.Message}");
            }
        }

    }
}