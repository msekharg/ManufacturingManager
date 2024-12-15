namespace ManufacturingManager.Core;

public interface ILoggingService
{
    void SetUpUserForLogger(string? userLogged, string? connectionString);
    void Fatal(string message, Exception? exception = null);
    void Error(string message, Exception? exception = null);
    void Debug(string message, Exception? exception = null);
    void Warning(string message, Exception? exception = null);
}