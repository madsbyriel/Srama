namespace Services;

public interface ILogging 
{
    void LogException(Exception exception);
    void LogException(Exception exception, params object?[]? toPrint);
}