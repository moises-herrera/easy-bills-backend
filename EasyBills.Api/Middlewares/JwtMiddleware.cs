using EasyBills.Security.Helpers;

namespace EasyBills.Api.Middlewares;

/// <summary>
/// Middleware to validate JWT token.
/// </summary>
public class JwtMiddleware
{
    /// <summary>
    /// Delegate of the request.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Object to get the configuration from appsettings.json
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtMiddleware"/> class.
    /// </summary>
    /// <param name="next">Request delegate.</param>
    /// <param name="configuration">Configuration object.</param>
    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    /// <summary>
    /// Validate the JWT token.
    /// </summary>
    /// <param name="context">Request context.</param>
    /// <returns>Task result.</returns>
    public async Task Invoke(HttpContext context)
    {
        var token = context.Request
                .Headers["Authorization"]
                .FirstOrDefault()?
                .Split(" ")
                .Last();
        var userId = JwtHelper.ValidateJWT(_configuration, token);

        if (userId != null)
        {
            context.Items["UserId"] = userId;
        }

        await _next(context);
    }
}

/// <summary>
/// Extensions class to setup JWT middleware.
/// </summary>
public static class JwtMiddlewareExtensions
{
    /// <summary>
    /// Configure the JWT middleware.
    /// </summary>
    /// <param name="builder">App builder</param>
    /// <returns>The app builder instance.</returns>
    public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<JwtMiddleware>();
    }
}