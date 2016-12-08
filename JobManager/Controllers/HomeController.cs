using System;
using System.Collections.Generic;
using System.Web.Mvc;
using JobManager.Helpers;
using JobManager.Models;
using System.Linq;

namespace JobManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Summary(string dbServer = null)
        {
            JobList jobs = new JobList();
            List<Models.JobSummaryModel> joblist = new List<Models.JobSummaryModel>();
            joblist = jobs.getJobs(dbServer);

            int jobsFound = 0;
            int jobsRunning = 0;
            int jobsSucceeded = 0;
            int jobsFailed = 0;
            int jobsDisabled = 0;

            foreach (var job in joblist)
            {
                jobsFound++;

                if (job.Status.Contains("Executing"))
                    jobsRunning++;

                if (job.Status.Equals("Failed"))
                    jobsFailed++;

                if (job.Status.Equals("Succeeded"))
                    jobsSucceeded++;

                if (!job.Enabled)
                    jobsDisabled++;
            }

            ViewBag.jobsFound = jobsFound;
            ViewBag.jobsRunning = jobsRunning;
            ViewBag.jobsSucceeded = jobsSucceeded;
            ViewBag.jobsFailed = jobsFailed;
            ViewBag.jobsDisabled = jobsDisabled;

            return View();
        }

        public PartialViewResult _LeftMenu()
        {
            List<IServers> servers = new List<IServers>();
            servers = GetConfig.GetServers();
            return PartialView(servers);
        }

    }
}