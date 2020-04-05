using System;
using System.Threading;

namespace Circle.Options
{
    public class CircleOptions
    {
        public CancellationToken CancellationToken { get; set; }
        public TimeSpan Period { get; set; }
        public Type HandlerType { get; set; }
        public int Test { get; set; }

        public void UseHandler<T>()
        {
            HandlerType = typeof(T);
        }
    }
}