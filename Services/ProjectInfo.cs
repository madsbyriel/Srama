namespace Services;
using System.Text.Json;

class ProjectInfo : IProjectInfo
{
    // Member variables
    private string hostname;
    private string logDirectory;
    private int port;

    // Properties
    public string Hostname => hostname;
    public int Port => port;
    public string LoggingDirectory => logDirectory;

    public ProjectInfo() 
    {
        FileInfo fileInfo = new FileInfo("project_settings.json");
        string jsonString = File.ReadAllText(fileInfo.FullName);
        ProjectJson projectJson = JsonSerializer.Deserialize<ProjectJson>(jsonString) ?? new ProjectJson();
        
        hostname = projectJson.hostname;
        port = projectJson.port;
        DirectoryInfo logDirectoryInfo = new DirectoryInfo(projectJson.log_directory);
        if (!logDirectoryInfo.Exists) {
            logDirectoryInfo.Create();
        }
        logDirectory = logDirectoryInfo.FullName;
    }

    public override string ToString()
    {
        return String.Format("Hostname: {0}\nPort: {1}\nLoggingDirectory: {2}", hostname, port, logDirectory);
    }

    private class ProjectJson {
        public string hostname { get; set; } = "";
        public int port { get; set; } = 0;
        public string log_directory { get; set; } = "";
    }
}

