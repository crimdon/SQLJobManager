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

        [HttpGet]
        public ActionResult StartJob (string dbServer, Guid jobID)
        {
            JobDetailsModel model = new JobDetailsModel();
            PopulateDropDowns dropdown = new PopulateDropDowns();
            ConnectSqlServer connection = new ConnectSqlServer();
            Server server = connection.Connect(dbServer);
            List<SelectListItem> stepList = new List<SelectListItem>();

            stepList = dropdown.getSteps(dbServer, jobID);
            ViewBag.StepList = stepList;

            var job = server.JobServer.GetJobByID(jobID);
            model.JobName = job.Name;
            model.ServerName = dbServer;
            model.JobID = jobID;
            model.StepName = job.JobSteps[0].Name;

            return View(model);
        }

        [HttpPost]
        public ActionResult StartJob (JobDetailsModel step)
        {
            ConnectSqlServer connection = new ConnectSqlServer();
            Server server = connection.Connect(step.ServerName);

            server.JobServer.GetJobByID(step.JobID).Start(step.StepName);
            server.ConnectionContext.Disconnect();
            return RedirectToAction("Index", "Schedule", new { dbServer = step.ServerName });
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