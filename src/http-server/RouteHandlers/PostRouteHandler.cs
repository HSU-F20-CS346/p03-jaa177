using System.Collections.Generic;
using System.IO;

namespace http_server.RouteHandlers
{
    public class PostRouteHandler : IRouteHandler
    {
        public void HandleRoute(RequestHeader header, Response response)
        {
            
            if(header.Route == "/login.html")
            {
                Dictionary<string, string> postValues = header.getData();
                if(postValues.ContainsKey("UserEmail") && postValues.ContainsKey("UserPassword"))
                {
                    if(postValues["UserEmail"] == "luckylogger@humboldt.edu" && postValues["UserPassword"] == "password")
                    {
                        string res_file = Path.Join(System.AppDomain.CurrentDomain.BaseDirectory, "public");
                        res_file = Path.Join(res_file, "success.html");
                        response.SetBody(File.ReadAllText(res_file));
                    } 
                    else
                    {
                        string res_file = Path.Join(System.AppDomain.CurrentDomain.BaseDirectory, "public");
                        res_file = Path.Join(res_file, "failure.html");
                        response.SetBody(File.ReadAllText(res_file));
                    }
                } 
                else
                {
                    //Error 400 Bad Request
                    response.Header.ResponseCode = 400;
                }
            }
            else
            {
                //throw an error
                response.Header.ResponseCode = 403; //Error 403 Forbidden
            }
        }
    }
}
