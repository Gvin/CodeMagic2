using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace CodeMagic.Core.Common
{
    public interface IPerformanceMeter
	{
        IDisposable Start(string methodName);
    }

    public class PerformanceMeter : IPerformanceMeter
    {
        public static IPerformanceMeter Instance { get; private set; }

        public static void Initialize(IPerformanceMeter instance)
        {
            Instance = instance;
        }

        private readonly ILogger<PerformanceMeter> _logger;

        public PerformanceMeter(ILogger<PerformanceMeter> logger)
        {
            _logger = logger;
        }

        public IDisposable Start(string methodName)
        {
            return new PerformanceCounter(time =>
            {
                _logger.LogDebug("{MethodName}: {Miliseconds}", methodName, time);
            });
        }

        private class PerformanceCounter : IDisposable
        {
            private readonly Stopwatch stopwatch;
            private readonly Action<long> callback;

            public PerformanceCounter(Action<long> callback)
            {
                this.callback = callback;
                stopwatch = Stopwatch.StartNew();
            }

            public void Dispose()
            {
                stopwatch.Stop();
                callback?.Invoke(stopwatch.ElapsedMilliseconds);
            }
        }
    }
}