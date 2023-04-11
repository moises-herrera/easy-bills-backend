using EasyBills.Core.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace EasyBills.Infrastructure.Data.Services;

/// <summary>
/// Email service for sending emails.
/// </summary>
public class EmailService : IEmailService
{
    /// <summary>
    /// Object to get the configuration from the appsettings.json
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initialize a new instance of <see cref="EmailService"/> class
    /// </summary>
    /// <param name="configuration">Configuration object.</param>
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Send email.
    /// </summary>
    /// <param name="to">Recipient email.</param>
    /// <param name="subject">Email subject.</param>
    /// <param name="body">Email body.</param>
    /// <returns>Task result.</returns>
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
