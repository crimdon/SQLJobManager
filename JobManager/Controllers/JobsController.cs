using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using JobManager.Helpers;
using SharkDev.Web.Controls.TreeView.Model;
using JobManager.Models;
using System.Linq;

namespace JobManager.Controllers
{
    public class JobsController : Controller
    {
        public ActionResult Index(string dbServer = null)
        {
            JobList jobs = new JobList();
            List<Models.JobSummaryModel> joblist = new List<Models.JobSummaryModel>();
            joblist = jobs.getJobs(dbServer);
            
            return View(joblist);
        }

        public PartialViewResult _LeftMenu()
        {
            List<IServers> servers = new List<IServers>();
            servers = GetConfig.GetServers();
            return PartialView(servers);
        }
    }
}