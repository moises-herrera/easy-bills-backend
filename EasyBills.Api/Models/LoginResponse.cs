using EasyBills.Application.Users;

namespace EasyBills.Api.Models;

/// <summary>
/// Login response model.
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// User information.
    /// </summary>
    public UserDTO User { get; set; }

    /// <summary>
    /// Access token.
    /// </summary>
    public string AccessToken { get; set; }
}