using JobManager.Data;
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
            ConfigModel db = new ConfigModel();
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(ServerName);

            string JobName = dbServer.JobServer.GetJobByID(JobID).Name;

            db.ActivityLogs.Add(new ActivityLog { DateTime = DateTime.Now, UserName = UserName, ServerName = ServerName, JobName = JobName, Action = Action });
            db.SaveChanges();
        }
    }
}