using EasyBills.Application.Email;
using EasyBills.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EasyBills.Api.Controllers;

/// <summary>
/// The controller to handle email actions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    /// <summary>
    /// The email service used to send emails.
    /// </summary>
    private readonly IEmailService _emailService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailController"/> class.
    /// </summary>
    /// <param name="emailService">The email service instance.</param>
    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    /// <summary>
    /// Confirm the email of a user.
    /// </summary>
    /// <param name="emailData">Email data.</param>
    /// <returns>Success response.</returns>
    [HttpPost("confirm-email")]
    public async Task<ActionResult> SendUserEmailConfirmation(
        EmailDTO emailData)
    {
        var baseDirectory = Environment.CurrentDirectory;
        var emailBody = System.IO.File.ReadAllText(
            $@"{baseDirectory}\EmailTemplates\ConfirmEmail.html");

        await _emailService.Send(
            emailData.Recipient, 
            "Verifica tu dirección de email", 
            emailBody);

        return Ok();
    }
}
