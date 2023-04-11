namespace EasyBills.Api.Middlewares;

/// <summary>
/// Middleware to log responses.
/// </summary>
public class LogResponseMiddleware
{
    /// <summary>
    /// Delegate of the request.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Logger instance.
    /// </summary>
    private readonly ILogger<LogResponseMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogResponseMiddleware"/> class.
    /// </summary>
    /// <param name="next">Request delegate.</param>
    /// <param name="logger">Logger instance.</param>
    public LogResponseMiddleware(RequestDelegate next, ILogger<LogResponseMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Log response information.
    /// </summary>
    /// <param name="context">Request context.</param>
    /// <returns>Task result.</returns>
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

/// <summary>
/// Extensions class to setup LogResponse middleware.
/// </summary>
public static class LogResponseMiddlewareExtensions
{
    /// <summary>
    /// Configure the log response middleware
    /// </summary>
    /// <param name="app">The app builder.</param>
    /// <returns>The app builder instance.</returns>
    public static IApplicationBuilder UseLogResponse(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LogResponseMiddleware>();
    }
}
