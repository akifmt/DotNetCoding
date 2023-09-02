using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Text;

namespace BlazorAppSendEmailwithMailKit.Data;

public class EmailSenderSystem : IEmailSender
{
    private readonly EmailConfiguration _emailConfiguration;

    public EmailSenderSystem(IOptions<EmailConfiguration> emailConfiguration)
    {
        this._emailConfiguration = emailConfiguration.Value;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return Execute(email, subject, htmlMessage);
    }

    private async Task Execute(string to, string subject, string htmlMessage)
    {
        string host = _emailConfiguration.Host;
        int port = _emailConfiguration.Port;
        string username = _emailConfiguration.Username;
        string password = _emailConfiguration.Password;
        string from = _emailConfiguration.From;
        string name = _emailConfiguration.Name;
        bool enableSsl = _emailConfiguration.EnableSSL;

        MailAddress sender = new MailAddress(from);
        if (!string.IsNullOrEmpty(name))
            sender = new MailAddress(from, name);
        MailAddress toEmail = new MailAddress(to);

        using MailMessage message = new MailMessage(sender, toEmail);
        message.Subject = subject;
        message.Body = htmlMessage;
        message.HeadersEncoding = Encoding.UTF8;
        message.SubjectEncoding = Encoding.UTF8;
        message.BodyEncoding = Encoding.UTF8;
        message.IsBodyHtml = true;

        using (var smtp = new SmtpClient(host))
        {
            smtp.Port = port;
            smtp.EnableSsl = enableSsl;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;

            if (!string.IsNullOrEmpty(username))
                smtp.Credentials = new System.Net.NetworkCredential(username, password);
            try
            {
                await smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}