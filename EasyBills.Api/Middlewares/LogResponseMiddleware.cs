namespace EasyBills.Api.Middlewares;

public class LogResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LogResponseMiddleware> _logger;

    public LogResponseMiddleware(RequestDelegate next, ILogger<LogResponseMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (var memoryStream = new MemoryStream())
        {
            var responseBody = context.Response.Body;
            context.Response.Body = memoryStream;

            await _next(context);

            memoryStream.Seek(0, SeekOrigin.Begin);
            string response = new StreamReader(memoryStream).ReadToEnd();
            memoryStream.Seek(0, SeekOrigin.Begin);

            await memoryStream.CopyToAsync(responseBody);
            context.Response.Body = responseBody;

            _logger.LogInformation(response);
        }
    }
}

public static class LogResponseMiddlewareExtensions
{
    public static IApplicationBuilder UseLogResponse(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LogResponseMiddleware>();
    }
}
