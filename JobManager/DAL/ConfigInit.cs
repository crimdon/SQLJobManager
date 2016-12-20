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
                new ServerConfig {ServerName = "SQLSERVER", AuthenticationType = AuthenticationType.SQL, UserName = "sa", Password = "as" }
            };

            dbServers.ForEach(s => context.ServerConfiguration.Add(s));
            context.SaveChanges();

            var editableCategories = new List<EditableCategories>
            {
                new EditableCategories {CategoryName = "Backups", Editable = false },               
                new EditableCategories {CategoryName =  "REPL-Alert Response", Editable = false},
                new EditableCategories {CategoryName =  "REPL-Checkup", Editable = false},
                new EditableCategories {CategoryName =  "REPL-Distribution", Editable = false},
                new EditableCategories {CategoryName =  "REPL-Distribution Cleanup", Editable = false},
                new EditableCategories {CategoryName =  "REPL-History Cleanup", Editable = false},
                new EditableCategories {CategoryName =  "REPL-LogReader", Editable = false},
                new EditableCategories {CategoryName =  "REPL-Snapshot", Editable = false},
                new EditableCategories {CategoryName =  "Database Maintenance", Editable = false},
                new EditableCategories {CategoryName =  "Report Server", Editable = false}
            };

            editableCategories.ForEach(s => context.EditableCategories.Add(s));
            context.SaveChanges();
        }
    }
}