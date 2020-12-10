using System;
using System.Collections.Generic;
using System.Text;

namespace http_server
{
    //see https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types 
    //for a list of MIME types
    public enum ContentType { HTML, X_Icon, JPEG, CSS, JavaScript, MP4, MP3, WAV, OGG, PNG, JSON, Text, CSV, GIF, TS, m3u8 };
    public enum RequestMethod { Unknown, GET, POST, PUT, DELETE };

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
                    return "text/css; charset=UTF-8";
                    break;
                case ContentType.JPEG:
                    return "image/jpeg";
                    break;
                case ContentType.X_Icon:
                    return "image/x-icon";
                    break;
                case ContentType.JavaScript:
                    return "application/javascript";
                    break;
                case ContentType.MP4:
                    return "video/mp4";
                    break;
                case ContentType.MP3:
                    return "audio/mpeg";
                    break;
                case ContentType.WAV:
                    return "audio/wav";
                    break;
                case ContentType.OGG:
                    return "audio/vorbis";
                    break;
                case ContentType.PNG:
                    return "image/png";
                    break;
                case ContentType.JSON:
                    return "application/json; charset=UTF-8";
                    break;
                case ContentType.Text:
                    return "text/plain; charset=UTF-8";
                    break;
                case ContentType.CSV:
                    return "text/csv; charset=UTF-8";
                    break;
                case ContentType.GIF:
                    return "image/gif";
                    break;
                case ContentType.TS:
                    return "video/MP2T";
                    break;
                case ContentType.m3u8:
                    return "application/x-mpegURL";
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
