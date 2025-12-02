using BackendAuthTemplate.Application.Common;
using BackendAuthTemplate.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BackendAuthTemplate.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            _ = services.AddLogging();

            _ = services.AddMediatR(cfg =>
            {
                _ = cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                _ = cfg.AddRequestPreProcessor(typeof(IRequestPreProcessor<>), typeof(LoggingBehavior<>));
                _ = cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
                _ = cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            _ = services.AddAutoMapper(Assembly.GetExecutingAssembly());

            _ = services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            _ = services.AddConfiguratorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
