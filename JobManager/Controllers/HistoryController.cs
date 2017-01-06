using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobManager.Models;
using JobManager.Helpers;

namespace JobManager.Controllers
{
    public class HistoryController : Controller
    {
        public ActionResult Index(string dbServer, Guid jobID)
        {
            List<JobHistorySummary> historyModel = new List<JobHistorySummary>();
            JobHistory jobHistory = new JobHistory();

            historyModel = jobHistory.getHistory(dbServer, jobID);

            return View(historyModel);
        }
    }
}