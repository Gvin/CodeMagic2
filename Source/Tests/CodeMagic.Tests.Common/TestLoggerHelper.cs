using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace CodeMagic.Tests.Common;

public static class TestLoggerHelper
{
    public static ILoggerFactory CreateLoggerFactory()
    {
        var logger = new LoggerConfiguration().WriteTo.NUnitOutput().CreateLogger();
        return new LoggerFactory(new[] { new SerilogLoggerProvider(logger) });
    }

    public static ILogger<T> CreateLogger<T>()
    {
        return CreateLoggerFactory().CreateLogger<T>();
    }
}