using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace JobManager.Helpers
{
    public class PopulateDropDowns
    {
        public List<SelectListItem> getDatabases (string ServerName)
        {
            List<SelectListItem> databases = new List<SelectListItem>();
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(ServerName);

            foreach (Database db in dbServer.Databases)
            {
                databases.Add(new SelectListItem { Text = db.Name, Value = db.Name });
            }

            return databases;
        }

        public List<SelectListItem> getActions (string ServerName, Guid JobID, int JobStep)
        {
            List<SelectListItem> actions = new List<SelectListItem>();
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(ServerName);

            actions.Add(new SelectListItem { Text = "Go to the next step", Value = "GoToNextStep"  } );
            actions.Add(new SelectListItem { Text = "Quit the job reporting success", Value = "QuitWithFailure" });
            actions.Add(new SelectListItem { Text = "Quit the job reporting failure", Value = "QuitWithSuccess" });

            Job job = dbServer.JobServer.GetJobByID(JobID);

            foreach (JobStep step in job.JobSteps)
            {
                if (step.ID != JobStep)
                    actions.Add(new SelectListItem { Text = "Go to step: [" + step.ID + "] " + step.Name, Value = "GoToStep:" + step.ID });
            }     

            return actions;
        }

        public List<SelectListItem> getFrequencyTypes()
        {
            List<SelectListItem> frequencyTypes = new List<SelectListItem>();

            frequencyTypes.Add(new SelectListItem { Text = "The schedule runs when the Microsoft SQL Server service is started.", Value = "AutoStart" });
            frequencyTypes.Add(new SelectListItem { Text = "The schedule runs every day.", Value = "Daily" });
            frequencyTypes.Add(new SelectListItem { Text = "The schedule runs every month.", Value = "Monthly" });
            frequencyTypes.Add(new SelectListItem { Text = "The schedule runs at a specified relative interval each month", Value = "MonthlyRelative" });
            frequencyTypes.Add(new SelectListItem { Text = "The schedule runs once only.", Value = "OneTime" });
            frequencyTypes.Add(new SelectListItem { Text = "The schedule runs when the processor is idle.", Value = "OnIdle" });                       
            frequencyTypes.Add(new SelectListItem { Text = "The schedule runs every week.", Value = "Weekly" });

            return frequencyTypes;
        }

        public List<SelectListItem> getSubdayTypes()
        {
            List<SelectListItem> subDayTypes = new List<SelectListItem>();

            subDayTypes.Add(new SelectListItem { Text = "hour(s)", Value = "hour" });
            subDayTypes.Add(new SelectListItem { Text = "minute(s)", Value = "minute" });
            subDayTypes.Add(new SelectListItem { Text = "second(s)", Value = "second" });

            return subDayTypes;
        }

        public List<SelectListItem> getFreqRelativeIntervals()
        {
            List<SelectListItem> freqRelativeIntervals = new List<SelectListItem>();

            freqRelativeIntervals.Add(new SelectListItem { Text = "First", Value = "First" });
            freqRelativeIntervals.Add(new SelectListItem { Text = "Second", Value = "Second" });
            freqRelativeIntervals.Add(new SelectListItem { Text = "Third", Value = "Third" });
            freqRelativeIntervals.Add(new SelectListItem { Text = "Fourth", Value = "fourth" });
            freqRelativeIntervals.Add(new SelectListItem { Text = "Last", Value = "Last" });

            return freqRelativeIntervals;
        }

        public List<SelectListItem> getMonthlyFrequency()
        {
            List<SelectListItem> monthlyFrequency = new List<SelectListItem>();

            monthlyFrequency.Add(new SelectListItem { Text = "Monday", Value = "Monday" });
            monthlyFrequency.Add(new SelectListItem { Text = "Tuesday", Value = "Tuesday" });
            monthlyFrequency.Add(new SelectListItem { Text = "Wednesday", Value = "Wednesday" });
            monthlyFrequency.Add(new SelectListItem { Text = "Thursday", Value = "Thursday" });
            monthlyFrequency.Add(new SelectListItem { Text = "Friday", Value = "Friday" });
            monthlyFrequency.Add(new SelectListItem { Text = "Saturday", Value = "Suturday" });
            monthlyFrequency.Add(new SelectListItem { Text = "Sunday", Value = "Sunday" });
            monthlyFrequency.Add(new SelectListItem { Text = "day", Value = "day" });
            monthlyFrequency.Add(new SelectListItem { Text = "weekday", Value = "weekday" });
            monthlyFrequency.Add(new SelectListItem { Text = "weekend day", Value = "weekendday" });

            return monthlyFrequency;
        }
    }
}