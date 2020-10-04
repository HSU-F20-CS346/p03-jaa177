using System.Net;
using System.Threading;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;

namespace http_server
{
    class Program
    {
        static bool createDb = false;
        static bool startHttp = false;
        static bool startHttps = false;
        static int httpPort = 80;
        static int httpsPort = 443;
        static string httpsCertificate;
        static string httpsCertificatePass;
        static void createSqliteDb()
        {
            HashAlgorithm sha = SHA256.Create();
            SQLiteConnection.CreateFile("UserDatabase.sqlite");

            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=UserDatabase.sqlite;Version=3;");
            m_dbConnection.Open();

            string sql = "create table users (username varchar(50), passhash varchar(50))";

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            string user = "luckylogger@humboldt.edu";
            byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes("password"));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            string pass = sBuilder.ToString();
            sql = "insert into users (username, passhash) values ($user, $pass)";

            command = new SQLiteCommand(sql, m_dbConnection);
            command.Parameters.AddWithValue("$user", user);
            command.Parameters.AddWithValue("$pass", pass);
            command.ExecuteNonQuery();

            m_dbConnection.Close();
        }

        static void Main(string[] args)
        {
            foreach(string arg in args)
            {
                int c = 1;
                switch (arg) {
                    case "--createDatabase":
                        createDb = true;
                        break;
                    case "--startHttp":
                        startHttp = true;
                        break;
                    case "--startHttps":
                        startHttps = true;
                        break;
                    case "--httpPort":
                        startHttp = true;
                        httpPort = Int32.Parse(args[c]);
                        break;
                    case "--httpsPort":
                        startHttps = true;
                        httpsPort = Int32.Parse(args[c]);
                        break;
                    case "--httpsCertificate":
                        startHttps = true;
                        httpsCertificate = args[c];
                        break;
                    case "--httpsCertificatePassword":
                        startHttps = true;
                        httpsCertificatePass = args[c];
                        break;
                    default:
                        Console.WriteLine("Invalid argument: {0}", arg);
                        break;
                }
                c++; //lol
            }

            if(createDb)
            {
                createSqliteDb();
            }
            if(startHttp)
            {
                HttpServer insecure = new HttpServer();
                insecure.Address = IPAddress.Any;
                insecure.Port = httpPort;

                ThreadStart httpStart = () => { insecure.Start(); };
                Thread httpThread = new Thread(httpStart);
                httpThread.Start();
            }

            if(startHttps)
            {
                HttpsServer server = new HttpsServer();
                server.Address = IPAddress.Any;
                server.Port = httpsPort;
                server.serverCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(Path.Join(Path.Join(System.AppDomain.CurrentDomain.BaseDirectory, "keys"), httpsCertificate), httpsCertificatePass);

                ThreadStart httpsStart = () => { server.Start(); };
                Thread httpsThread = new Thread(httpsStart);
                httpsThread.Start();
            }
        }
    }
}
