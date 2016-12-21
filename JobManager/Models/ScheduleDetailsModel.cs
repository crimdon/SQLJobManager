using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobManager.Models
{
    public class ScheduleDetailsModel : IValidatableObject
    {
        // Applies to all Schedules
        [Required]
        public string Name { get; set; }
        [Required]
        public string ScheduleFrequency { get; set; }
        public bool IsEnabled { get; set; }
        public string ServerName { get; set; }
        public Guid JobID { get; set; }
        public Guid ScheduleUID { get; set; }

        // OneTime Schedules
        [DataType(DataType.Date)]
        public DateTime OneTimeStartDate { get; set; }
        [DataType(DataType.Time)]
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
        [DataType(DataType.Time)]
        public TimeSpan DailyFreqOccursOnceTime { get; set; }
        public int DailyFreqOccursEvery { get; set; }
        public string DailyFreqSubDay { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan DailyFreqStartingTime { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan DailyFreqEndingTime { get; set; }


        // duration for Daily, Weekly and Monthly
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DurationStartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DurationEndDate { get; set; }
        public bool DurationNoEndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            switch (ScheduleFrequency)
            {
                case "OneTime":
                    if (OneTimeStartDate.Date < DateTime.Now.Date)
                    {
                        yield return new ValidationResult("Date invalid");
                    }
                    break;
                case "Daily":
                    if (DailyRecursEvery < 1)
                    {
                        yield return new ValidationResult("Daily frequency invalud");
                    }
                    break;
            }
        }
    }
}