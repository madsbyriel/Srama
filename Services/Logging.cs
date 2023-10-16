namespace Services;

public class Logging : ILogging
{
    DirectoryInfo loggingDirectoryInfo;
    public Logging(IProjectInfo projectInfo)
    {
        loggingDirectoryInfo = new DirectoryInfo(projectInfo.LoggingDirectory);
    }

    public void LogException(Exception exception)
    {
        string fileName = loggingDirectoryInfo.FullName + "/" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt";
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            writer.WriteLine(exception.ToString());
        }
    }

    public void LogException(Exception exception, params object?[]? toPrint)
    {
        if (toPrint != null) {
            foreach (object? item in toPrint)
            {
                Console.WriteLine(item);
            }
        }
        LogException(exception);
    }
}