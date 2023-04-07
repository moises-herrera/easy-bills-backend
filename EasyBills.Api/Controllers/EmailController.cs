using EasyBills.Application.Email;
using EasyBills.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EasyBills.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

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
