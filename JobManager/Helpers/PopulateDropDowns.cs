﻿using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using System;
using System.Collections.Generic;
using System.Data;
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

        public List<SelectListItem> getProxies (string ServerName, AgentSubSystem StepType)
        {
            List<SelectListItem> proxies = new List<SelectListItem>();
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(ServerName);

            foreach (ProxyAccount proxy in dbServer.JobServer.ProxyAccounts)
            {
                DataTable dt = proxy.EnumSubSystems();
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Name"].ToString() == StepType.ToString())
                    {
                        proxies.Add(new SelectListItem { Text = proxy.Name, Value = proxy.ID.ToString() });
                    }
                }
            }
            return proxies;
        }

        public List<SelectListItem> getSteps (string ServerName, Guid JobID)
        {
            List<SelectListItem> steps = new List<SelectListItem>();
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(ServerName);

            Job job = dbServer.JobServer.GetJobByID(JobID);

            foreach (JobStep step in job.JobSteps)
            {
                    steps.Add(new SelectListItem { Text = step.Name, Value = step.Name });
            }

            return steps;
        }

        public List<SelectListItem> getActions (string ServerName, Guid JobID, int JobStep)
        {
            List<SelectListItem> actions = new List<SelectListItem>();
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(ServerName);

            actions.Add(new SelectListItem { Text = "Go to the next step", Value = "GoToNextStep"  } );
            actions.Add(new SelectListItem { Text = "Quit the job reporting success", Value = "QuitWithSuccess" });
            actions.Add(new SelectListItem { Text = "Quit the job reporting failure", Value = "QuitWithFailure" });

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

            subDayTypes.Add(new SelectListItem { Text = "hour(s)", Value = "Hour" });
            subDayTypes.Add(new SelectListItem { Text = "minute(s)", Value = "Minute" });
            subDayTypes.Add(new SelectListItem { Text = "second(s)", Value = "Second" });

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
            monthlyFrequency.Add(new SelectListItem { Text = "day", Value = "EveryDay" });
            monthlyFrequency.Add(new SelectListItem { Text = "weekday", Value = "WeekDays" });
            monthlyFrequency.Add(new SelectListItem { Text = "weekend day", Value = "WeekEnds" });

            return monthlyFrequency;
        }
    }
}