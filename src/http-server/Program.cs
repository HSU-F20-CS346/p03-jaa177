using System;
using System.Net;

namespace http_server
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer server = new HttpServer();
            server.Address = IPAddress.Any;
            server.Port = 8080;
            server.Start();
        }
    }
}
