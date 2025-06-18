using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Interfaces;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace BackendAuthTemplate.API.Common.Swagger
{
    public class IncludeParameterFilter(IServiceProvider services) : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!context.ApiDescription.TryGetMethodInfo(out MethodInfo? methodInfo))
            {
                return;
            }

            IncludeParameterAttribute? includeAttr = methodInfo.GetCustomAttribute<IncludeParameterAttribute>()
                            ?? methodInfo.DeclaringType?.GetCustomAttribute<IncludeParameterAttribute>();
            if (includeAttr == null)
            {
                return;
            }

            OpenApiParameter? includeParam = operation.Parameters.FirstOrDefault(p => p.Name == "include" && p.In == ParameterLocation.Query);
            if (includeParam == null)
            {
                includeParam = new OpenApiParameter { Name = "include", In = ParameterLocation.Query, Required = false };
                operation.Parameters.Add(includeParam);
            }

            Type entityType = includeAttr.EntityType;
            Type configuratorType = typeof(IIncludeConfigurator<>).MakeGenericType(entityType);
            dynamic? configurator = services.GetService(configuratorType);

            HashSet<string> baseIncludes = configurator?.DefaultIncludes ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, HashSet<string>> roleExtras = configurator?.RoleExtras ?? new Dictionary<string, HashSet<string>>();

            HashSet<string> allowedIncludes = new(baseIncludes, StringComparer.OrdinalIgnoreCase);
            foreach (HashSet<string> roleSet in roleExtras.Values)
            {
                allowedIncludes.UnionWith(roleSet);
            }

            string defaultIncludesText = baseIncludes.Count != 0
                ? $"Default includes: {string.Join(", ", baseIncludes)}."
                : "No default includes.";

            string roleExtrasText = roleExtras.Count != 0
                ? " Role-based extras: " + string.Join("; ", roleExtras.Select(kvp => $"{kvp.Key}: {string.Join(", ", kvp.Value)}")) + "."
                : " No role-based extras.";

            includeParam.Description = $"Optional list of related entities to include. {defaultIncludesText}{roleExtrasText}";

            includeParam.Schema = new OpenApiSchema
            {
                Type = "array",
                Items = new OpenApiSchema
                {
                    Type = "string",
                    Enum = allowedIncludes
                        .Select(val => new OpenApiString(val))
                        .Cast<IOpenApiAny>()
                        .ToList()
                }
            };
        }
    }

}
