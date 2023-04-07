namespace EasyBills.Core.Interfaces;
public interface IEmailService
{
    Task Send(string to, string subject, string body);
}
