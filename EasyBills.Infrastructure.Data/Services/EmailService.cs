using EasyBills.Core.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace EasyBills.Infrastructure.Data.Services;
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task Send(string to, string subject, string body)
    {
        var appSettings = _configuration.GetSection("AppSettings");
        var emailHost = appSettings.GetSection("EmailHost").Value;
        int.TryParse(
            appSettings
            .GetSection("EmailPort").Value, out int port);
        var senderEmail = appSettings.GetSection("EmailUser").Value;
        var senderPassword = appSettings.GetSection("EmailPassword").Value;
        var email = new MimeMessage();

        email.From.Add(MailboxAddress.Parse(senderEmail));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = body };
        Console.WriteLine("Send");
        using var client = new SmtpClient();
        await client.ConnectAsync(emailHost, port, true);
        client.Authenticate(senderEmail, senderPassword);
        client.Send(email);
        client.Disconnect(true);
    }
}
