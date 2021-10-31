using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.WebSockets;
using System.Threading;

namespace Sockman.Mock.Web
{
    public class WebMockServer
    {
        public int Port { get; private set; }
        public HttpListener Listener { get; private set; }

        public WebMockServer(int port=8080)
        {
            Port = port;
            Listener = new HttpListener();
            Listener.Prefixes.Add($"http://*:{port}/");
        }

        public async Task Serve()
        {
            Listener.Start();
            while (true)
            {
                HttpListenerContext context = await Listener.GetContextAsync();
                Console.WriteLine($"request: {context.Request.Url}");
                if (context.Request.IsWebSocketRequest)
                {
                    HttpListenerWebSocketContext ws = await context.AcceptWebSocketAsync(null);
                    ws.WebSocket.Dispose();
                }
                else
                {
                    byte[] buffer = Encoding.UTF8.GetBytes("hello");
                    context.Response.OutputStream.Write(buffer);
                    context.Response.Close();
                }
            }
        }
    }
}
