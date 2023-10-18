using System.Net;

namespace Classes;

public class Server 
{
    private HttpListener listener;
    private string hostname;
    private int port;

    private Dictionary<string, OnRequestReceivedDel> routeToFunction;

    public Server(string hostname, int port)
    {
        this.routeToFunction = new Dictionary<string, OnRequestReceivedDel>();
        this.hostname = hostname;
        this.port = port;
        this.listener = new HttpListener();
    }

    public void StartServer() 
    {
        string httpString = string.Format("http://{0}:{1}/", hostname, port);
        listener.Prefixes.Add(httpString);
        listener.Start();
        Console.WriteLine("Server started at: \n\t" + httpString);
    }

    public Task ServerRun() 
    {
        return Task.Run(async () => {
            while (true)
            {
                HttpListenerContext context = await GetRequestAsync();
                string key = context.Request.HttpMethod;
                if (context.Request.Url != null) 
                {
                    key += context.Request.Url.AbsolutePath;
                }
                
                OnRequestReceivedDel? del;
                if (routeToFunction.TryGetValue(key, out del)) 
                {
                    Console.WriteLine("Success: " + key);
                    del?.Invoke(context);
                }
                else
                {
                    Console.WriteLine("Not found: " + key);
                }

                context.Response.Close();
            }
        });
    }

    private Task<HttpListenerContext> GetRequestAsync() 
    {
        return Task.Run(() => {
            return listener.GetContext();
        });
    }

    public static bool CheckPathAndMethod(HttpListenerContext context, string method, string? path) 
    {
        if (method != context.Request.HttpMethod) return false;
        
        if (context.Request.Url != null && path != null && path != context.Request.Url.AbsolutePath) 
        {
            return false;
        }

        if ((context.Request.Url != null && path == null) || (context.Request.Url == null && path != null))
        {
            return false;
        }

        return true;
    }

    public void AddRoute(string method, string path, OnRequestReceivedDel onRequestReceivedDel) 
    {
        string key = method + path;
        if (routeToFunction.ContainsKey(key)) 
        {
            throw new Exception("Route already exists here!");
        }
        routeToFunction[key] = onRequestReceivedDel;
    }
}

public delegate void OnRequestReceivedDel(HttpListenerContext context);