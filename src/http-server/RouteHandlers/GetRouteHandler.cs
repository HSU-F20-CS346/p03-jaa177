using System;
using System.IO;
using System.Collections.Generic;

namespace http_server.RouteHandlers
{
    public class GetRouteHandler : IRouteHandler
    {
        /// <summary>
        /// Sets the MIME type in the response header based on the file requested.  See
        /// <see cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types "/> for 
        /// a list of MIME types
        /// </summary>
        /// <param name="file"></param>
        /// <param name="header"></param>
        private void SetMimeType(string file, ResponseHeader header)
        {
            string ext = Path.GetExtension(file);
            switch(ext)
            {
                case ".css":
                    header.ContentType = ContentType.CSS;
                    break;
                case ".jpg":
                    header.ContentType = ContentType.JPEG;
                    break;
                case ".jpeg":
                    header.ContentType = ContentType.JPEG;
                    break;
                case ".html":
                    header.ContentType = ContentType.HTML;
                    break;
                case ".ico":
                    header.ContentType = ContentType.X_Icon;
                    break;
                case ".js":
                    header.ContentType = ContentType.JavaScript;
                    break;
                case ".json":
                    header.ContentType = ContentType.JSON;
                    break;
                case ".wav":
                    header.ContentType = ContentType.WAV;
                    break;
                case ".mp4":
                    header.ContentType = ContentType.MP4;
                    break;
                case ".mp3":
                    header.ContentType = ContentType.MP3;
                    break;
                case ".ogg":
                    header.ContentType = ContentType.OGG;
                    break;
                case ".png":
                    header.ContentType = ContentType.PNG;
                    break;
                case ".txt":
                    header.ContentType = ContentType.Text;
                    break;
                case ".csv":
                    header.ContentType = ContentType.CSV;
                    break;
                case ".gif":
                    header.ContentType = ContentType.GIF;
                    break;
                case ".ts":
                    header.ContentType = ContentType.TS;
                    break;
                case ".m3u8":
                    header.ContentType = ContentType.m3u8;
                    break;
            }
        }

        /// <summary>
        /// Constructs the appropriate response based on the supplied RequestHeader
        /// </summary>
        /// <param name="header"></param>
        /// <param name="response"></param>
        public void HandleRoute(RequestHeader header, Response response)
        {
            //predefined routes
            if (header.Route == "/echo")
            {
                //for debugging
                response.SetBody(header.ToString());
            }
            else if (header.Route == "/")
            {
                Dictionary<string, string> testDict = new Dictionary<string, string>();
                testDict.Add("test_cookie", "Hello, World!");
                response.SetBody(File.ReadAllText(Constants.DEFAULT_ROUTE));
                response.Header.setCookieData(testDict);
            }
            else
            {
                if (header.Route.Length > 0)
                {
                    Console.WriteLine(header.Route);
                    SetMimeType(header.Route, response.Header);
                    string path = Path.Join(System.AppDomain.CurrentDomain.BaseDirectory, "public");
                    path = Path.Join(path, header.Route);
                    switch(response.Header.ContentType)
                    {
                        case ContentType.HTML:
                            response.SetBody(File.ReadAllText(path));
                            break;
                        case ContentType.CSS:
                            response.SetBody(File.ReadAllText(path));
                            break;
                        case ContentType.JPEG:
                            response.SetBody(File.ReadAllBytes(path));
                            break;
                        case ContentType.X_Icon:
                            response.SetBody(File.ReadAllBytes(path));
                            break;
                        case ContentType.CSV:
                            response.SetBody(File.ReadAllText(path));
                            break;
                        case ContentType.JavaScript:
                            response.SetBody(File.ReadAllText(path));
                            break;
                        case ContentType.JSON:
                            response.SetBody(File.ReadAllText(path));
                            break;
                        case ContentType.MP3:
                            response.SetBody(File.ReadAllBytes(path));
                            break;
                        case ContentType.MP4:
                            response.SetBody(File.ReadAllBytes(path));
                            break;
                        case ContentType.OGG:
                            response.SetBody(File.ReadAllBytes(path));
                            break;
                        case ContentType.PNG:
                            response.SetBody(File.ReadAllBytes(path));
                            break;
                        case ContentType.Text:
                            response.SetBody(File.ReadAllText(path));
                            break;
                        case ContentType.WAV:
                            response.SetBody(File.ReadAllBytes(path));
                            break;
                        case ContentType.GIF:
                            response.SetBody(File.ReadAllBytes(path));
                            break;
                        case ContentType.TS:
                            response.SetBody(File.ReadAllBytes(path));
                            break;
                        case ContentType.m3u8:
                            response.SetBody(File.ReadAllText(path));
                            break;

                        default:
                            response.SetBody(File.ReadAllBytes(path));
                            break;
                    }
                }
                else
                {
                    //404 not found
                    response.Header.ResponseCode = 404;
                    response.SetBody(File.ReadAllText(Constants.ERROR_404));
                }
            }
        }
    }
}
