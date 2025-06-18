using BackendAuthTemplate.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace BackendAuthTemplate.Application.Common.Include
{
    public static class IncludeConfiguratorRegistration
    {
        public static IServiceCollection AddConfiguratorsFromAssembly(this IServiceCollection services, Assembly applicationAssembly)
        {
            var configurators = applicationAssembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Select(t => new
                {
                    Implementation = t,
                    Interface = t.GetInterfaces().FirstOrDefault(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IIncludeConfigurator<>))
                })
                .Where(x => x.Interface != null);

            foreach (var cfg in configurators)
            {
                _ = services.AddSingleton(cfg.Interface!, cfg.Implementation);
            }

            return services;
        }
    }

}
