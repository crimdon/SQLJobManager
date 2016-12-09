using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobManager.Models;
using JobManager.Helpers;

namespace JobManager.Controllers
{
    public class EditController : Controller
    {
        public ActionResult _LeftMenu(string dbServer, Guid jobID)
        {
            ViewBag.ServerName = dbServer;
            ViewBag.JobID = jobID;
            return PartialView();
        }

        [HttpGet]
        public ActionResult General(string dbServer, Guid jobID)
        {
            JobDetailsModel generalModel = new JobDetailsModel();
            JobDetails jobDetails = new JobDetails();

            generalModel = jobDetails.getGeneral(dbServer, jobID);

            ViewBag.ServerName = dbServer;
            ViewBag.JobID = jobID;
            return View(generalModel);
        }

        [HttpPost]
        public ActionResult General(JobDetailsModel jobDetail)
        {
            JobDetails jobDetails = new JobDetails();
            jobDetails.saveGeneral(jobDetail);
            TempData["message"] = "Job successfuly updated";

            return RedirectToAction("General", "Edit", new { dbServer = jobDetail.ServerName, jobID = jobDetail.JobID});
        }

        public ActionResult Steps(string dbServer, Guid jobID)
        {
            List<JobStepListModel> steps = new List<JobStepListModel>();
            JobSteps jobSteps = new JobSteps();

            steps = jobSteps.getSteps(dbServer, jobID);
            ViewBag.ServerName = dbServer;
            ViewBag.JobID = jobID;
            return View(steps);
        }

        public ActionResult DeleteStep (string dbServer, Guid jobID, int stepID)
        {
            JobSteps jobSteps = new JobSteps();
            jobSteps.deleteStep(dbServer, jobID, stepID);
            return RedirectToAction("Steps", "Edit", new { dbServer = dbServer, jobID = jobID });
        }

        public ActionResult Schedules(string dbServer, Guid jobID)
        {
            List<JobScheduleListModel> schedules = new List<JobScheduleListModel>();
            JobSchedules jobschedules = new JobSchedules();

            schedules = jobschedules.getSchedules(dbServer, jobID);
            ViewBag.ServerName = dbServer;
            ViewBag.JobID = jobID;
            return View(schedules);
        }

        public ActionResult DeleteSchedule (string dbServer, Guid jobID, Guid scheduleUID)
        {
            JobSchedules jobschedules = new JobSchedules();
            jobschedules.deleteSchedule(dbServer, jobID, scheduleUID);
            ViewBag.ServerName = dbServer;
            ViewBag.JobID = jobID;
            return RedirectToAction("Schedules", "Edit", new { dbServer = dbServer, jobID = jobID });
        }

        [HttpGet]
        public ActionResult EditStep (string dbServer, Guid jobID, int stepID)
        {
            JobSteps jobSteps = new JobSteps();
            PopulateDropDowns dropdown = new PopulateDropDowns();
            StepDetailsModel step = new StepDetailsModel();
            List<SelectListItem> databaseList = new List<SelectListItem>();
            List<SelectListItem> actionList = new List<SelectListItem>();

            databaseList = dropdown.getDatabases(dbServer);
            ViewBag.DatabaseList = databaseList;

            actionList = dropdown.getActions(dbServer, jobID, stepID);
            ViewBag.ActionList = actionList;

            ViewBag.ServerName = dbServer;
            ViewBag.JobID = jobID;
            step = jobSteps.getStepDetails(dbServer, jobID, stepID);
            return View(step);
        }

        [HttpPost]
        public ActionResult EditStep(StepDetailsModel step)
        {
            JobSteps jobSteps = new JobSteps();

            jobSteps.saveStepDetails(step);

            ViewBag.ServerName = step.ServerName;
            ViewBag.JobID = step.JobID;
            return RedirectToAction("Steps", "Edit", new { dbServer = step.ServerName, jobID = step.JobID });
        }
    }
}