using System;

namespace http_server
{
    public class ResponseHeader
    {
        public ContentType ContentType { get; set; }

        //see https://developer.mozilla.org/en-US/docs/Web/HTTP/Status for a list of codes
        public int ResponseCode { get; set; }

        /// <summary>
        /// Should match the length of the response body in bytes
        /// </summary>
        public int ContentLength { get; set; }

        public DateTime Date { get; set; }

        public ResponseHeader()
        {
            ResponseCode = 200;
        }

        public override string ToString()
        {

            string header = string.Format("HTTP/1.1 {0}\r\n" +
                            "Content-type: {1}\r\n" +
                            "Cache-Control: max-age=0\r\n" +
                            "Date: {2}\r\n" +
                            "Server: My custom server\r\n" +
                            "Content-Length: {3}\r\n" +
                            "Connection: Close\r\n\r\n",
                            ResponseCode,
                            ContentType.ToHeader(),
                            Date.ToUniversalTime().ToString("ddd, dd MM yyyy HH:mm:ss GMT"),
                            ContentLength
                            );
            return header;
        }
    }
}
