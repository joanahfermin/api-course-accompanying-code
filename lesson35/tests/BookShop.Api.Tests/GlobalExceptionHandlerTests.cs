using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BookShop.Api.Tests;

[TestClass]
public class GlobalExceptionHandlerTests
{
    private FakeLogger<GlobalExceptionHandler> _logger = null!;
    private GlobalExceptionHandler _handler = null!;
    private DefaultHttpContext _context = null!;

    [TestInitialize]
    public void Setup()
    {
        _logger = new FakeLogger<GlobalExceptionHandler>();
        _handler = new GlobalExceptionHandler(_logger);
        _context = new DefaultHttpContext();
        _context.Response.Body = new MemoryStream();
    }

    private async Task<string> ReadBodyAsync()
    {
        _context.Response.Body.Position = 0;
        return await new StreamReader(_context.Response.Body).ReadToEndAsync();
    }

    [TestMethod]
    public async Task TryHandleAsync_AnyException_Answers500AsProblemDetails()
    {
        var handled = await _handler.TryHandleAsync(
            _context, new InvalidOperationException("boom"), CancellationToken.None);

        Assert.IsTrue(handled);
        Assert.AreEqual(500, _context.Response.StatusCode);
        Assert.StartsWith("application/problem+json", _context.Response.ContentType);
        var body = JsonDocument.Parse(await ReadBodyAsync());
        Assert.AreEqual(500, body.RootElement.GetProperty("status").GetInt32());
    }

    [TestMethod]
    public async Task TryHandleAsync_ExceptionWithSecret_DoesNotLeakItToTheClient()
    {
        var exception = new InvalidOperationException("Cannot open Data Source=secret.db");

        await _handler.TryHandleAsync(_context, exception, CancellationToken.None);

        var body = await ReadBodyAsync();
        Assert.DoesNotContain("secret.db", body);
        Assert.DoesNotContain("InvalidOperationException", body);
    }

    [TestMethod]
    public async Task TryHandleAsync_AnyException_LogsItAsAnError()
    {
        var exception = new InvalidOperationException("boom");

        await _handler.TryHandleAsync(_context, exception, CancellationToken.None);

        Assert.HasCount(1, _logger.Levels);
        Assert.AreEqual(LogLevel.Error, _logger.Levels[0]);
        Assert.AreSame(exception, _logger.Exceptions[0]);
    }
}
