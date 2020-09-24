using System;
using System.Collections.Generic;
using System.Text;

namespace http_server
{
    //see https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types 
    //for a list of MIME types
    public enum ContentType { HTML, X_Icon, JPEG, CSS };
    public enum RequestMethod { Unknown, GET, POST };

    static class ContentTypeExtensions
    {
        public static string ToHeader(this ContentType type)
        {
            switch (type)
            {
                case ContentType.HTML:
                    return "text/html; charset=UTF-8";
                    break;
                case ContentType.CSS:
                    return "text/css";
                    break;
                case ContentType.JPEG:
                    return "image/jpeg";
                    break;
                case ContentType.X_Icon:
                    return "image/x-icon";
                    break;

                //TODO: fill in additional cases for all content types
                //see https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types 
                //for appropriate strings
                default:
                    return "application/octet-stream";
                    break;
            }
        }
    }
}
