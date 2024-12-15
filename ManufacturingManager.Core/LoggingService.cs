using log4net;

namespace ManufacturingManager.Core;

public class LoggingService : ILoggingService
{
    private readonly ILog _logger;
    private string? _userLogged;
    private string? _connectionString;

    public LoggingService()
    {
        _logger = LogManager.GetLogger(GetType());
    }

    public void SetUpUserForLogger(string? userLogged, string? connectionString)
    {
        _userLogged = userLogged;
        _connectionString = connectionString;
    }

    public void Fatal(string message, Exception? exception = null)
    {
        _logger?.Fatal(message, exception);
    }

    public void Error(string message, Exception? exception = null)
    {
        _logger.Error(message, exception);
    }

    public void Debug(string message, Exception? exception = null)
    {
        _logger.Debug(message, exception);
    }

    public void Warning(string message, Exception? exception = null)
    {
        _logger.Warn(message, exception);
    }
}