using EasyBills.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EasyBills.Api.Authorization;

/// <summary>
/// Authorization attribute for controllers.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizationAttribute : Attribute, IAuthorizationFilter
{
    /// <summary>
    /// Confirm if the request is authorized.
    /// </summary>
    /// <param name="context">Authorization context</param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor
            .EndpointMetadata
            .OfType<AllowAnonymousAttribute>().Any();

        if (allowAnonymous) return;

        var userId = context.HttpContext.Items["UserId"];

        if (userId is null)
        {
            context.Result = new JsonResult(new ErrorResponse { Error = "Unauthorized" })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }
}
