using System.Net;
using Services;

InterfaceProvider serviceProvider = new InterfaceProvider();
serviceProvider.AddStaticService<IProjectInfo, ProjectInfo>();

HttpListener httpListener = new HttpListener();
httpListener.Prefixes.Add("http://localhost:31415/");

httpListener.Start();
Console.WriteLine("Server started");

var assembly = System.Reflection.Assembly.GetExecutingAssembly();
var attributes = assembly.GetCustomAttributes(false);
foreach (var attribute in attributes)
{
    Console.WriteLine(attribute.ToString());
}

while (true) 
{
    HttpListenerContext context = httpListener.GetContext();
    Console.WriteLine(context.Request.HttpMethod);
    if (context.Request.Url != null) 
    {
        Console.WriteLine(context.Request.Url.AbsolutePath);
    } else
    {
        Console.WriteLine("Twas null.");
    }
}