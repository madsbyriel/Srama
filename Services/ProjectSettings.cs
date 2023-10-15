namespace Services;
using System.Text.Json;

class ProjectInfo : IProjectSettings
{
    public string hostname { get; set; }
    public int port { get; set; }

    public ProjectInfo() 
    {
        FileInfo fileInfo = new FileInfo("project_settings.json");
        string jsonString = File.ReadAllText(fileInfo.FullName);
        ProjectJson projectJson = JsonSerializer.Deserialize<ProjectJson>(jsonString) ?? new ProjectJson();
        
        hostname = projectJson.hostname;
        port = projectJson.port;
    }

    public override string ToString()
    {
        return hostname + " " + port;
    }

    public string getHostname() 
    {
        return hostname;
    }

    public int getPort() 
    {
        return port;
    }
    private class ProjectJson {
        public string hostname { get; set; } = "";
        public int port { get; set; } = 0;
    }
}

