using System.IO;

namespace http_server.RouteHandlers
{
    public class DeleteRouteHandler : IRouteHandler
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
                File.Delete(path);
                response.Header.ResponseCode = 200;
            }
        }
    }
}
