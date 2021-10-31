using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Sockman.Mock.IPMock
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
