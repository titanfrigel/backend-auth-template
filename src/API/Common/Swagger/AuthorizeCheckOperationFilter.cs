using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BackendAuthTemplate.API.Common.Swagger
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            object[]? controllerAttributes = context.MethodInfo.DeclaringType?.GetCustomAttributes(true);
            object[] methodAttributes = context.MethodInfo.GetCustomAttributes(true);

            bool allowAnonymous = methodAttributes.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
            {
                operation.Description += "\n\n✅ **No Authentication Required**";
                return;
            }

            List<AuthorizeAttribute>? authAttributes = controllerAttributes?.Union(methodAttributes)
                .OfType<AuthorizeAttribute>()
                .ToList();

            if (authAttributes != null && authAttributes.Count == 0)
            {
                operation.Security =
                [
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new List<string>()
                        }
                    }
                ];

                List<string?> roles = authAttributes
                    .Where(a => !string.IsNullOrEmpty(a.Roles))
                    .Select(a => a.Roles)
                    .Distinct()
                    .ToList();

                if (roles.Count == 0)
                {
                    operation.Description += $"\n\n🔒 **Authentication Required**";
                    operation.Description += $"\n🔹 **Required Roles:** `{string.Join(", ", roles)}`";
                }
                else
                {
                    operation.Description += "\n\n🔒 **Authentication Required (Any Authenticated User)**";
                }
            }
            else
            {
                operation.Description += "\n\n✅ **No Authentication Required**";
            }
        }
    }
}
