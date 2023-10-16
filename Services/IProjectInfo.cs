namespace Services;

public interface IProjectInfo 
{
    string Hostname { get; }
    int Port { get; }
    string LoggingDirectory { get; }
}