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

            string subject = Language.Get("EmailAlertSubject", symbol);
            string body = EmailTemplate.LoadTemplate(symbol, price.ToString("F2", CultureInfo.InvariantCulture), action.ToString());

            try
            {
                foreach (var recipient in config.Recipients ?? new List<string>())
                {
                    var mail = new MailMessage
                    {
                        From = new MailAddress(config.Smtp.Email),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true,
                        BodyEncoding = System.Text.Encoding.UTF8,
                        SubjectEncoding = System.Text.Encoding.UTF8
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
