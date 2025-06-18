using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Reflection;

namespace BackendAuthTemplate.API.Common.Extensions
{
    public static class HubEndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder MapHubs(this IEndpointRouteBuilder endpoints)
        {
            IEnumerable<Type> hubTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(Hub).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (Type? hubType in hubTypes)
            {
                RouteAttribute? routeAttribute = hubType.GetCustomAttribute<RouteAttribute>();
                string routeTemplate;

                if (routeAttribute != null && !string.IsNullOrWhiteSpace(routeAttribute.Template))
                {
                    routeTemplate = routeAttribute.Template;
                }
                else
                {
                    string hubName = hubType.Name.EndsWith("Hub")
                        ? hubType.Name[..^"Hub".Length]
                        : hubType.Name;

                    routeTemplate = $"/v1/hubs/{hubName.ToLower()}";
                }

                MethodInfo? method = typeof(HubEndpointRouteBuilderExtensions)
                    .GetMethod(nameof(MapHubGeneric), BindingFlags.NonPublic | BindingFlags.Static)
                    ?.MakeGenericMethod(hubType);

                _ = method?.Invoke(null, [endpoints, routeTemplate]);
            }

            return endpoints;
        }

        private static void MapHubGeneric<THub>(IEndpointRouteBuilder endpoints, string route) where THub : Hub
        {
            _ = endpoints.MapHub<THub>(route);
        }
    }
}
