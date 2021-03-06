﻿using System.IO;

namespace http_server.RouteHandlers
{
    public class PutRouteHandler : IRouteHandler
    {
        public void HandleRoute(RequestHeader header, Response response)
        {
            if (header.Route == "/")
            {
                response.Header.ResponseCode = 403;
            }
            else
            {
                string path = Path.Join(System.AppDomain.CurrentDomain.BaseDirectory, "public");
                path = Path.Join(path, header.Route);
                string filedata = header.getPutData();
                File.WriteAllText(path, filedata);
                response.Header.ResponseCode = 200;
            }
        }
    }
}
