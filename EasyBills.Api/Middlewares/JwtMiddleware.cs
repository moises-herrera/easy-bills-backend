using EasyBills.Security.Helpers;

namespace EasyBills.Api.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

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

public static class JwtMiddlewareExtensions
{
    public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<JwtMiddleware>();
    }
}