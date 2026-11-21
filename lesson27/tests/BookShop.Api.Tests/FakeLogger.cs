using Microsoft.Extensions.Logging;

namespace BookShop.Api.Tests;

public class FakeLogger<T> : ILogger<T>
{
    public List<LogLevel> Levels { get; } = new List<LogLevel>();
    public List<Exception?> Exceptions { get; } = new List<Exception?>();

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
        Exception? exception, Func<TState, Exception?, string> formatter)
    {
        Levels.Add(logLevel);
        Exceptions.Add(exception);
    }
}
