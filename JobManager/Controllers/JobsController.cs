using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using JobManager.Helpers;
using JobManager.Models;
using System.Linq;
using JobManager.DAL;

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
            ConfigContext db = new ConfigContext();
            return PartialView(db.ServerConfiguration.ToList());
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            TempData["ErrorMessage"] = filterContext.Exception.Message;
            filterContext.Result = RedirectToAction("Error", "Home");
        }
    }
}