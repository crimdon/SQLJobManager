using JobManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobManager.DAL
{
    public class ConfigInit : System.Data.Entity. DropCreateDatabaseIfModelChanges<ConfigContext>
    {
        protected override void Seed(ConfigContext context)
        {
            var dbServers = new List<ServerConfig>
            {
                new ServerConfig {ServerName = "SQLSERVER", AuthenticationType = AuthenticationType.SQL, UserName = "sa", Password = "sa" }
            };

            dbServers.ForEach(s => context.ServerConfiguration.Add(s));
            context.SaveChanges();
        }
    }
}