using System;
using System.Threading;
using Circle.Options;
using Circle.Workers;
using Microsoft.Extensions.Options;

namespace Circle
{
    public class CircleControl
    {
        private readonly CircleOptions _options;
        private readonly CircleService _circleService;

        public CircleControl(IOptions<CircleOptions> options, CircleService circleService)
        {
            _options = options.Value;
            _circleService = circleService;
        }
        
        public void Start()
        {
            _circleService.StartAsync(new CancellationToken());
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public void Stop()
        {
            _circleService.StopAsync(new CancellationToken());
        }

        public void StopAfter(TimeSpan afterTimespan)
        {
            _circleService.StopAsync(new CancellationTokenSource(afterTimespan).Token);
        }

        public void ChangePeriod(TimeSpan newPeriod)
        {
            _options.Period = newPeriod;
        }
    }
}