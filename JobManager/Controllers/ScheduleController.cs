using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using JobManager.Helpers;
using JobManager.Models;
using JobManager.DAL;
using System.Linq;

namespace JobManager.Controllers
{
    public class ScheduleController : Controller
    {
        public ActionResult Index(string dbServer = null)
        {
            JobList jobs = new JobList();
            List<Models.JobSummaryModel> joblist = new List<Models.JobSummaryModel>();
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