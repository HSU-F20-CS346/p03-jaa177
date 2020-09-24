using http_server.RouteHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace http_server
{
    public class HttpServer
    {
        public IPAddress Address { get; set; }
        public int Port { get; set; }

        public void Start()
        {
            TcpListener listener = new TcpListener(Address, Port);
            Console.WriteLine("Listening for HTTP on port {0}", Port);
            listener.Start();
            while (true)
            {
                try
                {
                    using (var client = listener.AcceptTcpClient())
                    {
                        //some browsers like to hold onto the connection, so set a 1s timeout for sending and receiving
                        client.ReceiveTimeout = 1000;
                        client.SendTimeout = 1000;

                        //A using statement should automatically flush when it goes out of scope
                        using (BufferedStream stream = new BufferedStream(client.GetStream()))
                        {
                            BinaryReader reader = new BinaryReader(stream);

                            //8K header limit seems to be the norm for most real web servers
                            //https://www.tutorialspoint.com/What-is-the-maximum-size-of-HTTP-header-values
                            byte[] buffer = new byte[8192];
                            int bytesRead = reader.Read(buffer, 0, buffer.Length);
                            string requestHeaderString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            RequestHeader request = RequestHeader.FromString(requestHeaderString);

                            Console.WriteLine("Client {0} requests {1}", client.Client.RemoteEndPoint, request);

                            Response response = new Response();
                            IRouteHandler handler = null;
                            try
                            {
                                handler = RouteHandlerFactory.ConstructRouteHandler(request);
                                handler.HandleRoute(request, response);
                            }
                            catch(Exception ex)
                            {
                                //error constructing resonse
                                response.Header.ResponseCode = 500;
                                response.SetBody(File.ReadAllText(Constants.ERROR_500));
                                Console.WriteLine("error responding to client: {0}", ex.Message);
                            }
                            response.FlushToStream(client.GetStream());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("error responding to client: {0}", ex.Message);
                }

            }

        }
    }
}
