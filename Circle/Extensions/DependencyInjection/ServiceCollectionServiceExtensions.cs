using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Circle.Interfaces;
using Circle.Options;
using Circle.Workers;

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

            // new HostBuilder()    //This creates a new web host
            //     .conf
            //     .UseKestrel()       //Use the Kestrel web server that we just added with NuGet
            //     .UseContentRoot(Directory.GetCurrentDirectory())   // Use the current working as root directory for the host 
            //     .UseStartup<Startup>()  //Use Startup class for application configuration
            //     .Build()    //Build the server
            //     .Run();
            
            // var host = new HostBuilder()
            //     .ConfigureServices(services =>
            //     {
            //         services.AddControllers();
            //     })
            //     .Configure(app =>
            //     {
            //         app.UseRouting();
            //     
            //         app.UseEndpoints(endpoints =>
            //         {
            //             endpoints.MapControllerRoute(
            //                 name: "default",
            //                 pattern: "{controller=Home}/{action=Index}/{id?}");
            //         });
            //     })
            //     .Build();
            //
            // var host = new WebHostBuilder()
            //     .UseKestrel()
            //     .UseContentRoot(Directory.GetCurrentDirectory())
            //     .ConfigureServices(services =>
            //     {
            //         services.AddControllers();
            //     })
            //     .Configure(app =>
            //     {
            //         app.UseRouting();
            //
            //         app.UseEndpoints(endpoints =>
            //         {
            //             endpoints.MapControllerRoute(
            //                 name: "default",
            //                 pattern: "{controller=Home}/{action=Index}/{id?}");
            //         });
            //     })
            //     .Build();
            
            // host.Run();
            
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