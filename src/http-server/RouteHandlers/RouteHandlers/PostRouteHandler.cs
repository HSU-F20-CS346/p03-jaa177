using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System;

namespace http_server.RouteHandlers
{
    public class PostRouteHandler : IRouteHandler
    {
        public void HandleRoute(RequestHeader header, Response response)
        {
            HashAlgorithm sha = SHA256.Create();
            string r_user = "";
            string r_pass = "";

            if (header.Route == "/login.html")
            {
                Dictionary<string, string> postValues = header.getData();
                if (postValues.ContainsKey("UserEmail") && postValues.ContainsKey("UserPassword"))
                {
                    var connection = new SQLiteConnection("Data Source=UserDatabase.sqlite;Version=3;");
                    connection.Open();

                    string query = "SELECT * FROM users WHERE username=$username AND passhash=$pass";
                    SQLiteCommand qCommand = new SQLiteCommand(query, connection);
                    qCommand.Parameters.AddWithValue("$username", postValues["UserEmail"]);

                    byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes(postValues["UserPassword"]));
                    var sBuilder = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    {
                        sBuilder.Append(data[i].ToString("x2"));
                    }
                    string passwordHash = sBuilder.ToString();

                    qCommand.Parameters.AddWithValue("$pass", passwordHash);

                    SQLiteDataReader dbReader = qCommand.ExecuteReader();
                    while(dbReader.Read())
                    {
                        r_user = dbReader.GetString(0);
                        r_pass = dbReader.GetString(1);
                        Console.WriteLine("{0}: {1}", postValues["UserEmail"], r_user);
                        Console.WriteLine("{0}: {1}", passwordHash, r_pass);
                    }

                    if (r_user == postValues["UserEmail"] && r_pass == passwordHash)
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
                    Console.WriteLine("{0}: {1}", postValues["UserEmail"], r_user);
                    Console.WriteLine("{0}: {1}", passwordHash, r_pass);

                    connection.Close();

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