using EasyBills.Api.Models;
using EasyBills.Application.Email;
using EasyBills.Core.Interfaces;
using EasyBills.Domain.Interfaces;
using EasyBills.Security.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
    /// The user repository to handle user actions.
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Object used to get the configuration from the appsettings.
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailController"/> class.
    /// </summary>
    /// <param name="emailService">The email service instance.</param>
    /// <param name="userRepository">The user repository instance.</param>
    /// <param name="configuration">The object to get the configuration from the appsettings.json</param>
    public EmailController(IEmailService emailService, IUserRepository userRepository, IConfiguration configuration)
    {
        _emailService = emailService;
        _userRepository = userRepository;
        _configuration = configuration;
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

        try
        {
            var user = await _userRepository.GetOne(user => user.Email == emailData.Recipient);
            var token = JwtHelper.CreateJWT(_configuration, user.Id.ToString(), user.FullName, user.Email, Constants.emailVerificationTokenLifeTimeInMinutes);
            var frontendUrl = _configuration.GetSection("AppSettings").GetSection("FrontendUrl").Value;
            emailBody = emailBody.Replace("%FRONTEND_URL%", $"{frontendUrl}");
            emailBody = emailBody.Replace("%VERIFY_LINK%", $"{frontendUrl}/auth/confirm-email?userId={user.Id}&token={token}");
            await _emailService.Send(
                emailData.Recipient,
                "Verifica tu dirección de email",
                emailBody);

            return Ok();
        }
        catch (Exception ex)
        {
            var message = "Ha ocurrido un error al enviar el email";
            return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { Error = message, Exception = ex.Message });
        }
    }
}
