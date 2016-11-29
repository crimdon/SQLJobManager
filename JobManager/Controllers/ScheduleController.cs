using System;
using System.Collections.Generic;
using System.Web.Mvc;
using JobManager.Models;
using JobManager.Helpers;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Globalization;

namespace JobManager.Controllers
{
    public class ScheduleController : Controller
    {
        // GET: Schedule
        public ActionResult Index(string dbServer = null)
        {
            List<JobSummary> joblist = new List<JobSummary>();
            joblist = getJobs(dbServer);
            return View(joblist);
        }

        private string jobStatus;
        private string lastOutcome;
        private DateTime lastRun;
        private DateTime nextRun;
        private List<JobSummary> getJobs(string selectedServer)
        {
            Servers servers = new Servers();

            List<JobSummary> joblist = new List<JobSummary>();


            foreach (string server in servers.Serverlist)
            {
                if (string.IsNullOrEmpty(selectedServer) || selectedServer == server)
                {
                    ConnectSqlServer connection = new ConnectSqlServer();
                    Server dbServer = connection.Connect(server);
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

                        joblist.Add(new JobSummary
                        {
                            ServerName = row["originating_server"].ToString(),
                            JobName = row["name"].ToString(),
                            Enabled = Convert.ToBoolean(row["enabled"]),
                            Status = jobStatus,
                            LastOutcome = lastOutcome,
                            LastRun = lastRun,
                            NextRun = nextRun,
                            Category = row["category"].ToString(),
                            Scheduled = Convert.ToBoolean(row["has_schedule"])
                        });
                    }

                }
            }
            return joblist;
        }
    }
}