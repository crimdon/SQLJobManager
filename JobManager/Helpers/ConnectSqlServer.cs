using System;
using System.Linq;
using Microsoft.SqlServer.Management.Smo;
using JobManager.Data;

namespace JobManager.Helpers
{
    public class ConnectSqlServer
    {
        public Server Connect(string SqlServer)
        {
            ConfigModel db = new ConfigModel();
            try
            {
                ServerConfig serverConfig = db.ServerConfigs.FirstOrDefault(m => m.ServerName == SqlServer);
                if (serverConfig == null)
                {
                    throw new Exception("Server not found in configuration");
                }
                Server server = new Server(serverConfig.ServerName);
                server.ConnectionContext.ConnectTimeout = 3;
                server.ConnectionContext.ApplicationName = "Index Manager";
                switch (serverConfig.AuthenticationType.ToString())
                {
                    case "SQL":
                        server.ConnectionContext.LoginSecure = false;
                        server.ConnectionContext.Login = serverConfig.UserName;
                        server.ConnectionContext.Password = serverConfig.Password;
                        break;
                    case "Windows":
                        server.ConnectionContext.LoginSecure = true;
                        server.ConnectionContext.ConnectAsUser = true;
                        server.ConnectionContext.ConnectAsUserName = serverConfig.UserName;
                        server.ConnectionContext.ConnectAsUserPassword = serverConfig.Password;
                        break;
                }
                string cs = server.ConnectionContext.ConnectionString.ToString(); 
                server.ConnectionContext.Connect();

                if (!server.ConnectionContext.FixedServerRoles.ToString().Any("SysAdmin".Contains))
                {
                    throw new Exception("User is not a member of SysAdmin");
                }

                return server;
            }

            catch (Exception)
            {
                throw;
            }

        }
    }
}