using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace EasyBills.Api.Authorization;

public class AddRequiredHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor descriptor)
        {
            if (!context.ApiDescription.CustomAttributes().Any((a) => a is AllowAnonymousAttribute)
                && (context.ApiDescription.CustomAttributes().Any((a) => a is AuthorizationAttribute)
                    || descriptor.ControllerTypeInfo.GetCustomAttribute<AuthorizationAttribute>() != null))
            {
                operation.Security ??= new List<OpenApiSecurityRequirement>();

                operation.Security.Add(
                    new OpenApiSecurityRequirement() {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                            BearerFormat = "Bearer token",
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            }
        }
    }
}
