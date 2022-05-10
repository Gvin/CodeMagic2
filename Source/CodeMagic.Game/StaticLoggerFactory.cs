using System;
using Microsoft.Extensions.Logging;

namespace CodeMagic.Game;

public static class StaticLoggerFactory
{
    private static ILoggerFactory _loggerFactory;

    public static void Initialize(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public static ILogger<T> CreateLogger<T>()
    {
        if (_loggerFactory == null)
        {
            throw new Exception($"{nameof(StaticLoggerFactory)} was not initialized.");
        }

        return _loggerFactory.CreateLogger<T>();
    }
}