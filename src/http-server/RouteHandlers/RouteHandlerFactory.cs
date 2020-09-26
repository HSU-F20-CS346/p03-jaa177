using System;
using System.Collections.Generic;
using System.Text;

namespace http_server.RouteHandlers
{
    public class RouteHandlerFactory
    {
        public static IRouteHandler ConstructRouteHandler(RequestHeader request)
        {
            switch (request.Method)
            {
                case RequestMethod.GET:
                    return new GetRouteHandler();
                    break;

                //PA3 TODO: Add case for RequestMethod.POST
                case RequestMethod.POST:
                    return new PostRouteHandler();
                    break;

                default:
                    throw new Exception("Request method not supported");
                    break;
            }
        }
    }
}
