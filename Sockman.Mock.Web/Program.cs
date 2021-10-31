using System;

namespace Sockman.Mock.Web
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                WebMockServer server = new WebMockServer();
                Console.WriteLine("Mock Http Server");
                server.Serve().Wait();
            }
            catch (Exception e)
            {

            }
        }
    }
}
