namespace http_server.RouteHandlers
{
    public interface IRouteHandler
    {
        void HandleRoute(RequestHeader header, Response response);
    }
}
