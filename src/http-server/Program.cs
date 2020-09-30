using System;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;

namespace http_server
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpsServer server = new HttpsServer();
            server.Address = IPAddress.Any;
            server.Port = 443;
            server.serverCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(Path.Join(Path.Join(System.AppDomain.CurrentDomain.BaseDirectory, "keys"), "legalizecrack.pfx"), "legalizecrack");
            //server.Start();

            HttpServer insecure = new HttpServer();
            insecure.Address = IPAddress.Any;
            insecure.Port = 80;
            //insecure.Start();

            ThreadStart httpStart = () => { server.Start(); };
            Thread httpThread = new Thread(httpStart);
            httpThread.Start();

            ThreadStart httpsStart = () => { insecure.Start(); };
            Thread httpsThread = new Thread(httpsStart);
            httpsThread.Start();
        }
    }
}
