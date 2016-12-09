using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace JobManager.Helpers
{
    public class PopulateDropDowns
    {
        public List<SelectListItem> getDatabases (string ServerName)
        {
            List<SelectListItem> databases = new List<SelectListItem>();
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(ServerName);

            foreach (Database db in dbServer.Databases)
            {
                databases.Add(new SelectListItem { Text = db.Name, Value = db.Name });
            }

            return databases;
        }

        public List<SelectListItem> getActions (string ServerName, Guid JobID, int JobStep)
        {
            List<SelectListItem> actions = new List<SelectListItem>();
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(ServerName);

            actions.Add(new SelectListItem { Text = "Go to the next step", Value = "GoToNextStep"  } );
            actions.Add(new SelectListItem { Text = "Quit the job reporting success", Value = "QuitWithFailure" });
            actions.Add(new SelectListItem { Text = "Quit the job reporting failure", Value = "QuitWithSuccess" });

            Job job = dbServer.JobServer.GetJobByID(JobID);

            foreach (JobStep step in job.JobSteps)
            {
                if (step.ID != JobStep)
                    actions.Add(new SelectListItem { Text = "Go to step: [" + step.ID + "] " + step.Name, Value = "GoToStep:" + step.ID });
            }     

            return actions;
        }
    }
}