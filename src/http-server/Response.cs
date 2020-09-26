using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace http_server
{
    public class Response
    {
        public ResponseHeader Header { get; set; }

        private byte[] _body;
        public byte[] Body
        {
            get
            {
                return _body;
            }
            protected set
            {
                _body = value;
                Header.ContentLength = _body.Length;
            }
        }

        public bool IsBinary
        {
            get
            {
                switch (Header.ContentType)
                {
                    case ContentType.JPEG:
                    case ContentType.X_Icon:
                        return true;
                        break;
                    default:
                        return false;
                        break;
                }

            }
        }

        public Response()
        {
            Header = new ResponseHeader();
        }

        public void SetBody(string text)
        {
            Body = Encoding.UTF8.GetBytes(text);
        }

        public void SetBody(byte[] bytes)
        {
            Body = bytes;
        }
         
        public void FlushToStream(Stream s)
        {
            s.Write(Encoding.UTF8.GetBytes(Header.ToString()));
            s.Write(Body);
        }

    }
}
