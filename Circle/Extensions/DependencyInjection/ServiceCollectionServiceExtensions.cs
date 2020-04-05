using System;
using System.Linq;
using System.Reflection;
using Circle.Interfaces;
using Circle.Options;
using Circle.Workers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Circle.Extensions.DependencyInjection
{
    public static class ServiceCollectionServiceExtensions
    {
        /// <summary>
        /// Add Circle to the current project
        /// </summary>
        public static IServiceCollection AddCircle(this IServiceCollection services, Action<CircleOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Configure(setupAction);
            
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptions<CircleOptions>>().Value;

            var workHandler = options.HandlerType.GetTypeInfo().Assembly.DefinedTypes
                .Where(t => typeof(IWorkHandler).GetTypeInfo().IsAssignableFrom(t.AsType()) && t.IsClass)
                .Select(p => p.AsType())
                .FirstOrDefault();

            if (workHandler == null)
            {
                throw new ArgumentException("You need to define a work handler that inherit from IWorkHandler.");
            }

            services.AddSingleton(workHandler);
            services.AddHostedService<CircleService>();
            services.AddSingleton<CircleService>();
            services.AddSingleton<CircleControl>();

            return services;
        }
    }
}

// services.AddHostedService<WorkService>();
// services.TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService, THostedService>());