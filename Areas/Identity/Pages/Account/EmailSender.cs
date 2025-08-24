using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Microlab.web.Models;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _settings;

    public EmailSender(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlMessage };

        using var client = new MailKit.Net.Smtp.SmtpClient(); // 👈 aqui é o nome completo
        await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_settings.FromEmail, _settings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
