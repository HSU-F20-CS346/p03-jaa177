using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace http_server
{
    
    public class RequestHeader
    {
        public RequestMethod Method { get; set; }
        public string Route { get; set; }
        public static Dictionary<string,string> Data { get; set; }
        public static Dictionary<string, string> CookieData { get; set; }

        public RequestHeader()
        {
            Method = RequestMethod.Unknown;
            Data = new Dictionary<string, string>();
        }

        //This will grab the query string (the string that contains the data in a POST request), which is just the last line in the request header.
        public static string GetQueryStringFromHeader(string data)
        {
            string[] vars = data.Split("\n");
            return vars[vars.Length - 1];
        }

        public static void getCookieData(string rawHeader)
        {
            string[] lines = rawHeader.Split("\n");
            foreach(var l in lines)
            {
                if(l.Contains("Cookie: "))
                {
                    string[] data = l.Split(": ");
                    string[] cookieData = data[1].Split("; ");
                    foreach(var d in cookieData)
                    {
                        string[] kv = d.Split("=");
                        CookieData.Add(kv[0], kv[1]);
                    }
                }
            }
        }

        public static void setPutData(string qs)
        {
            Data.Add("reqput_file_contents", qs);
        }
        //Sets the Data variable. Only run if there is a POST request.
        public static void SetData(string qs)
        {
            string[] vars = qs.Split("&");
            foreach(string k in vars)
            {
                string[] kt = k.Split("=");

                //All the Percent-encoded characters
                //From https://en.wikipedia.org/wiki/Percent-encoding
                kt[1] = kt[1].Replace("%21", "!");
                kt[1] = kt[1].Replace("%23", "#");
                kt[1] = kt[1].Replace("%24", "$");
                kt[1] = kt[1].Replace("%25", "%");
                kt[1] = kt[1].Replace("%26", "&");
                kt[1] = kt[1].Replace("%27", "\'");
                kt[1] = kt[1].Replace("%28", "(");
                kt[1] = kt[1].Replace("%29", ")");
                kt[1] = kt[1].Replace("%2A", "*");
                kt[1] = kt[1].Replace("%2B", "+");
                kt[1] = kt[1].Replace("%2C", ",");
                kt[1] = kt[1].Replace("%2F", "/");
                kt[1] = kt[1].Replace("%3A", ":");
                kt[1] = kt[1].Replace("%3B", ";");
                kt[1] = kt[1].Replace("%3D", "=");
                kt[1] = kt[1].Replace("%3F", "?");
                kt[1] = kt[1].Replace("%40", "@");
                kt[1] = kt[1].Replace("%5B", "[");
                kt[1] = kt[1].Replace("%5D", "]");
                Data.Add(kt[0], kt[1]);
            }
        }

        //Accessor function for the Data variable
        public Dictionary<string, string> getData()
        {
            return Data;
        }

        public string getPutData()
        {
            return Data["reqput_file_contents"];
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Method.ToString(), Route);
        }

        public static RequestHeader FromString(string rawHeader)
        {
            RequestHeader header = new RequestHeader();

            //https://regexr.com/ is a great site for testing new REGEX
            Regex getMethodPattern = new Regex(@"GET ([\/\w\d.]*)", RegexOptions.Compiled);
            var getMethodMatch = getMethodPattern.Match(rawHeader);
            if(getMethodMatch.Success == true)
            {
                header.Method = RequestMethod.GET;
                header.Route = getMethodMatch.Groups[1].Value;
                getCookieData(rawHeader);
            }

            Regex postMethodPattern = new Regex(@"POST ([\/\w\d.]*)", RegexOptions.Compiled);
            var postMethodMatch = postMethodPattern.Match(rawHeader);
            if (postMethodMatch.Success == true)
            {
                header.Method = RequestMethod.POST;
                header.Route = postMethodMatch.Groups[1].Value;
                getCookieData(rawHeader);
                SetData(GetQueryStringFromHeader(rawHeader));   
            }

            Regex putMethodPattern = new Regex(@"PUT ([\/\w\d.]*)", RegexOptions.Compiled);
            var putMethodMatch = putMethodPattern.Match(rawHeader);
            if (putMethodMatch.Success == true)
            {
                header.Method = RequestMethod.PUT;
                header.Route = postMethodMatch.Groups[1].Value;
                getCookieData(rawHeader);
                setPutData(GetQueryStringFromHeader(rawHeader));
            }
            Regex deleteMethodPattern = new Regex(@"DELETE ([\/\w\d.]*)", RegexOptions.Compiled);
            var deleteMethodMatch = deleteMethodPattern.Match(rawHeader);
            if (deleteMethodMatch.Success == true)
            {
                header.Method = RequestMethod.DELETE;
                header.Route = postMethodMatch.Groups[1].Value;
                getCookieData(rawHeader);

            }

            return header;
        }

    }
}
