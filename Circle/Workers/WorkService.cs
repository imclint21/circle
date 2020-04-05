using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Circle.Interfaces;
using Circle.Options;
using ClintSharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Circle.Workers
{
    public class CircleService : BackgroundService
    {
        private readonly CircleOptions _options;
        private readonly ILogger<CircleService> _logger;
        private readonly dynamic _work;

        public CircleService(IServiceProvider serviceProvider, ILogger<CircleService> logger, IOptions<CircleOptions> options)
        {
            _options = options.Value;
            _logger = logger;
            var workHandler = _options.HandlerType.GetTypeInfo().Assembly.DefinedTypes
                .Where(t => typeof(IWorkHandler).GetTypeInfo().IsAssignableFrom(t.AsType()) && t.IsClass)
                .Select(p => p.AsType())
                .FirstOrDefault();
            _work = serviceProvider.GetRequiredService(workHandler);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting Circle Service [period={_options.Period}]");
            
            while (!_options.CancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Executing work");
                _work.DoWork();
                await Task.Delay(_options.Period, _options.CancellationToken);
                
                if (_options.OnceLaunch)
                {
                    break;
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _options.CancellationToken = new CancellationTokenSource(TimeSpan.Zero).Token;
            _logger.LogInformation($"Stopping Circle Service");
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}