using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using JobManager.Models;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;


namespace JobManager.Helpers
{
    public class JobHistory
    {
        public List<JobHistorySummary> getHistory(string serverName, Guid jobID)
        {
            List<JobHistoryModel> stepHistory = new List<JobHistoryModel>();
            List<JobHistorySummary> jobHistory = new List<JobHistorySummary>();
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(serverName);
            JobHistoryFilter jhf = new JobHistoryFilter();

            Job jb = dbServer.JobServer.GetJobByID(jobID);
            jhf.JobID = jb.JobID;

            DataTable dt = jb.EnumHistory(jhf);
            dt.DefaultView.Sort = "InstanceID ASC";
            dt = dt.DefaultView.ToTable();
            foreach (DataRow row in dt.Rows)
            {
                if (int.Parse(row["StepID"].ToString()) != 0)
                {
                    stepHistory.Add(new JobHistoryModel
                    {
                        ServerName = serverName,
                        JobID = jobID,
                        JobName = row["JobName"].ToString(),
                        StepID = int.Parse(row["StepID"].ToString()),
                        ExecutionTime = DateTime.Parse(row["RunDate"].ToString()),
                        Duration = TimeSpan.FromSeconds(double.Parse(row["RunDuration"].ToString())),
                        Message = row["Message"].ToString(),
                        StepName = row["StepName"].ToString()
                    });
                }
                else
                {
                    jobHistory.Add(new JobHistorySummary
                    {
                        ServerName = serverName,
                        JobID = jobID,
                        JobName = row["JobName"].ToString(),
                        StepID = int.Parse(row["StepID"].ToString()),
                        ExecutionTime = DateTime.Parse(row["RunDate"].ToString()),
                        Duration = TimeSpan.FromSeconds(double.Parse(row["RunDuration"].ToString())),
                        Message = row["Message"].ToString(),
                        StepName = row["StepName"].ToString(),
                        JobHistories = stepHistory.OrderByDescending(i => i.StepID).ToList()
                    });
                    stepHistory.Clear();
                }
            }

            return jobHistory;
        }
    }
}