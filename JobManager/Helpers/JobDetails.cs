using System;
using JobManager.Models;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;

namespace JobManager.Helpers
{
    public class JobDetails
    {
        public JobDetailsModel getGeneral (string ServerName, Guid JobID)
        {
            JobDetailsModel jobDetails = new JobDetailsModel();

            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(ServerName);

            var job = dbServer.JobServer.GetJobByID(JobID);

            jobDetails.JobID = JobID;
            jobDetails.JobName = job.Name;
            jobDetails.Enabled = job.IsEnabled;
            jobDetails.Description = job.Description;
            jobDetails.Owner = job.OwnerLoginName;
            jobDetails.Created = job.DateCreated;
            jobDetails.LastExecuted = job.LastRunDate;
            jobDetails.LastModified = job.DateLastModified;
            jobDetails.ServerName = ServerName;
            jobDetails.StartStepID = job.StartStepID;
            jobDetails.StepCount = job.JobSteps.Count;


            return jobDetails;
        }

        public void saveGeneral(JobDetailsModel job)
        {
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(job.ServerName);

            Job jobToUpdate = dbServer.JobServer.GetJobByID(job.JobID);

            if (jobToUpdate.Name != job.JobName)
                jobToUpdate.Rename(job.JobName);
            jobToUpdate.OwnerLoginName = job.Owner;
            jobToUpdate.Description = job.Description;
            jobToUpdate.IsEnabled = job.Enabled;
            jobToUpdate.StartStepID = job.StartStepID;
            jobToUpdate.Alter();
        }
    }
}