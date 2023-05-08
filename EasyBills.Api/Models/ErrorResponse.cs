namespace EasyBills.Api.Models;

/// <summary>
/// Error response model.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Error message.
    /// </summary>
    public string Error { get; set; }

    /// <summary>
    /// Exception message.
    /// </summary>
    public string? Exception { get; set; }
}
