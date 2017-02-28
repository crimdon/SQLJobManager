using JobManager.DAL;
using JobManager.Models;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobManager.Helpers
{
    public class LogActivity
    {
        public void Add(string UserName, string ServerName, Guid JobID, string Action)
        {
            ConfigContext db = new ConfigContext();
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(ServerName);

            string JobName = dbServer.JobServer.GetJobByID(JobID).Name;

            db.Activity.Add(new ActivityLog { DateTime = DateTime.Now, UserName = UserName, ServerName = ServerName, JobName = JobName, Action = Action });
            db.SaveChanges();
        }
    }
}