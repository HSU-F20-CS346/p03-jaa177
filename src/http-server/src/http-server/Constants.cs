using System;
using System.Collections.Generic;
using System.Text;

namespace http_server
{
    public class Constants
    {
        #region Error Routes
        public static string ERROR_404 = "errors/404.html";
        public static string ERROR_500 = "errors/500.html";
        #endregion



        #region Special Routes

        public static string DEFAULT_PREFIX = "public";

        //not needed for simple HTML pages
        public static string DEFAULT_ROUTE = "public/index.html";
        #endregion
    }
}
