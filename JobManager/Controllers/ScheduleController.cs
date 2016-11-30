﻿using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using JobManager.Helpers;
using JobManager.Models;



namespace JobManager.Controllers
{
    public class ScheduleController : Controller
    {
        public ActionResult Index(string dbServer = null)
        {
            JobList jobs = new JobList();
            List<Models.JobSummary> joblist = new List<Models.JobSummary>();
            joblist = jobs.getJobs(dbServer);
            return View(joblist);
        }

        public ActionResult StartJob (string dbServer, Guid jobID)
        {
            ConnectSqlServer connection = new ConnectSqlServer();
            Server server = connection.Connect(dbServer);

            server.JobServer.GetJobByID(jobID).Start();
            server.ConnectionContext.Disconnect();
            return RedirectToAction("Index", "Schedule", new { dbServer = dbServer });
        }
    }
}