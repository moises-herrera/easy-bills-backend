namespace EasyBills.Core.Interfaces;

/// <summary>
/// Email service interface.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Send an email.
    /// </summary>
    /// <param name="to">Recipient email.</param>
    /// <param name="subject">Subject.</param>
    /// <param name="body">Email body.</param>
    /// <returns>Task result.</returns>
    Task Send(string to, string subject, string body);
}
