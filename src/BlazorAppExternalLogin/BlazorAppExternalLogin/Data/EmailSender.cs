using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Text;

namespace BlazorAppExternalLogin.Data;

public class EmailSender : IEmailSender
{
    private ILogger<EmailSender> _logger;

    public EmailSender(ILogger<EmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        _logger.LogInformation("Email sender started.");

        string host = "localhost";
        string from = "admin@admin.com";
        int port = 25;
        bool enableSsl = false;

        MailMessage message = new MailMessage(from, email, subject, htmlMessage);
        message.HeadersEncoding = Encoding.UTF8;
        message.SubjectEncoding = Encoding.UTF8;
        message.BodyEncoding = Encoding.UTF8;
        message.IsBodyHtml = true;

        using (var smtp = new SmtpClient(host))
        {
            smtp.Port = port;
            smtp.EnableSsl = enableSsl;

            try
            {
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Sending Email Error");
            }
        }

        return Task.CompletedTask;
    }
}