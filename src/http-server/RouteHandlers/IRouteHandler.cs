using System;
using System.Collections.Generic;
using System.Text;

namespace http_server.RouteHandlers
{
    public interface IRouteHandler
    {
        void HandleRoute(RequestHeader header, Response response);
    }
}
