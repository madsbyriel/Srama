using System.Net;
using System.Text;
using Classes;
using Services;

InterfaceProvider serviceProvider = new InterfaceProvider();
serviceProvider.AddStaticService<IProjectInfo, ProjectInfo>();

Server server = new Server("localhost", 31415);

server.AddRoute("GET", "/", context => {
    byte[] fileContents = File.ReadAllBytes("dist/main.js");

    // Set the content type of the response
    context.Response.ContentType = "text/plain";

    context.Response.StatusCode = 200;

    context.Response.OutputStream.Write(fileContents, 0, fileContents.Length);
    context.Response.OutputStream.Close();
});

server.StartServer();

Task serverRunTask = server.ServerRun();

await serverRunTask;