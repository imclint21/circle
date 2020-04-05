using System;
using System.Threading;

namespace Circle.Options
{
    public class CircleOptions
    {
        internal Type HandlerType { get; set; }
        
        public CancellationToken CancellationToken { get; set; }
        public TimeSpan Period { get; set; } = TimeSpan.FromSeconds(30);
        public bool OnceLaunch { get; set; }

        public void UseHandler<T>()
        {
            HandlerType = typeof(T);
        }
    }
}