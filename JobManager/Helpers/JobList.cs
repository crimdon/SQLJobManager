using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Globalization;
using JobManager.Models;
using System.Linq;

namespace JobManager.Helpers
{
    public class JobList
    {
        private string jobStatus;
        private string lastOutcome;
        private DateTime lastRun;
        private DateTime nextRun;
        private bool runable;
        public List<JobSummaryModel> getJobs(string selectedServer = null)
        {
            List<IServers> servers = GetConfig.GetServers();


            List<JobSummaryModel> joblist = new List<JobSummaryModel>();


            foreach (var server in servers)
            {
                if (string.IsNullOrEmpty(selectedServer) || selectedServer == server.ServerName)
                {
                    ConnectSqlServer connection = new ConnectSqlServer();
                    Server dbServer = connection.Connect(server.ServerName);
                    DataSet ds = dbServer.ConnectionContext.ExecuteWithResults("exec msdb.dbo.sp_help_job");

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        switch (row["current_execution_status"].ToString())
                        {
                            case "1":
                                jobStatus = "Executing:" + row["current_execution_step"];
                                break;
                            case "2":
                                jobStatus = "Waiting for Thread";
                                break;
                            case "3":
                                jobStatus = "Between Retries";
                                break;
                            case "4":
                                jobStatus = "Idle";
                                break;
                            case "5":
                                jobStatus = "Suspended";
                                break;
                            case "6":
                                jobStatus = "[Obsolete]";
                                break;
                            case "7":
                                jobStatus = "PerformingCompletionActions";
                                break;
                            default:
                                jobStatus = "";
                                break;

                        }

                        switch (row["last_run_outcome"].ToString())
                        {
                            case "0":
                                lastOutcome = "Failed";
                                break;
                            case "1":
                                lastOutcome = "Succeeded";
                                break;
                            case "2":
                                lastOutcome = "Retry";
                                break;
                            case "3":
                                lastOutcome = "Canceled";
                                break;
                            default:
                                lastOutcome = "Unknown";
                                break;
                        }

                        switch (row["last_run_date"].ToString())
                        {
                            case "0":
                                lastRun = DateTime.MinValue;
                                break;
                            default:
                                lastRun = DateTime.ParseExact(row["last_run_date"].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture)
                                    .Add(TimeSpan.ParseExact(row["last_run_time"].ToString().PadLeft(6, '0'), "hhmmss", CultureInfo.InvariantCulture));
                                break;
                        }

                        switch (row["next_run_date"].ToString())
                        {
                            case "0":
                                nextRun = DateTime.MinValue;
                                break;
                            default:
                                nextRun = DateTime.ParseExact(row["next_run_date"].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture)
                                    .Add(TimeSpan.ParseExact(row["next_run_time"].ToString().PadLeft(6, '0'), "hhmmss", CultureInfo.InvariantCulture));
                                break;
                        }

                        if (row["has_step"].ToString() != "0")
                            runable = true;
                        else
                            runable = false;

                        joblist.Add(new JobSummaryModel
                        {
                            JobID = Guid.Parse(row["job_id"].ToString()),
                            ServerName = row["originating_server"].ToString(),
                            JobName = row["name"].ToString(),
                            Enabled = Convert.ToBoolean(row["enabled"]),
                            Status = jobStatus,
                            LastOutcome = lastOutcome,
                            LastRun = lastRun,
                            NextRun = nextRun,
                            Category = row["category"].ToString(),
                            Runable = runable,
                            Scheduled = Convert.ToBoolean(row["has_schedule"]),
                            Description = row["description"].ToString(),
                            Owner = row["owner"].ToString()
                        });
                    }
                    dbServer.ConnectionContext.Disconnect();
                }
            }
            var sortedList = joblist.OrderBy(o => o.JobName).ToList();
            return sortedList;
        }
    }
}