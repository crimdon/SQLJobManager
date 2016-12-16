using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobManager.Models
{
    public class ScheduleDetailsModel
    {
        // Applies to all Schedules
        public string Name { get; set; }
        public string ScheduleFrequency { get; set; }
        public bool IsEnabled { get; set; }
        public string ServerName { get; set; }
        public Guid JobID { get; set; }
        public Guid ScheduleUID { get; set; }

        // OneTime Schedules
        public DateTime OneTimeStartDate { get; set; }
        public TimeSpan OneTimeStartTimeOfDay { get; set; }


        // Daily Schedules
        public int DailyRecursEvery { get; set; }


        // Weekly Schedules
        public int WeeklyRecursEvery { get; set; }
        public bool WeeklyMonday { get; set; }
        public bool WeeklyTuesday { get; set; }
        public bool WeeklyWednesday { get; set; }
        public bool WeeklyThursday { get; set; }
        public bool WeeklyFriday { get; set; }
        public bool WeeklySaturday { get; set; }
        public bool WeeklySunday { get; set; }


        // Monthly Schedules
        public int MonthlyDayNo { get; set; }
        public int MonthlyFrequency { get; set; }


        // Monthly Relative Schedules
        public int MonthlyRelativeFreq { get; set; }
        public string MonthlyRelativeFreqSubDayType { get; set; }
        public string MonthlyRelativeSubFreq { get; set; }
        


        // Daily Frequency for Daily, Weekly and Monthly
        public bool DailyFreqOccursOnce { get; set; }
        public TimeSpan DailyFreqOccursOnceTime { get; set; }
        public int DailyFreqOccursEvery { get; set; }
        public string DailyFreqSubDay { get; set; }
        public TimeSpan DailyFreqStartingTime { get; set; }
        public TimeSpan DailyFreqEndingTime { get; set; }


        // duration for Daily, Weekly and Monthly
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DurationStartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DurationEndDate { get; set; }
        public bool DurationNoEndDate { get; set; }
    }
}