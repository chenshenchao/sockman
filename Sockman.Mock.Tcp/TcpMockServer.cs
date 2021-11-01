using System;
using System.Net;
using System.Net.Sockets;

namespace Sockman.Mock.Tcp
{
    public class TcpMockServer
    {
        public TcpListener Listener { get; private set; }
        public TcpMockConfiguration Configuration { get; private set; }

        public TcpMockServer(TcpMockConfiguration configuration)
        {
            Configuration = configuration;
            Listener = new TcpListener(IPAddress.Any, configuration.Port);
        }

    }
}
