using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobManager.Models
{
    public class JobScheduleDetailsModel : IValidatableObject
    {
        // Applies to all Schedules
        [Required]
        [Display(Name = "Job name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Schedule type")]
        public string ScheduleFrequency { get; set; }
        [Display(Name = "Enabled")]
        public bool IsEnabled { get; set; }
        public string ServerName { get; set; }
        public Guid JobID { get; set; }
        public Guid ScheduleUID { get; set; }

        // OneTime Schedules
        [Required]
        [Display(Name = "Start date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime OneTimeStartDate { get; set; }
        [Required]
        [Display(Name = "Start time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan OneTimeStartTimeOfDay { get; set; }


        // Daily Schedules
        [Required]
        [Display(Name = "Daily frequency")]
        [Range(typeof(int), "1", "100", ErrorMessage = "Number must be between 1 and 100.")]
        public int DailyRecursEvery { get; set; }


        // Weekly Schedules
        [Required]
        [Display(Name = "Weekly frequency")]
        [Range(typeof(int), "1", "100", ErrorMessage = "Number must be between 1 and 100.")]
        public int WeeklyRecursEvery { get; set; }
        public bool WeeklyMonday { get; set; }
        public bool WeeklyTuesday { get; set; }
        public bool WeeklyWednesday { get; set; }
        public bool WeeklyThursday { get; set; }
        public bool WeeklyFriday { get; set; }
        public bool WeeklySaturday { get; set; }
        public bool WeeklySunday { get; set; }


        // Monthly Schedules
        [Required]
        [Display(Name = "Monthly day number")]
        [Range(typeof(int), "1", "31", ErrorMessage = "Number must be between 1 and 31.")]
        public int MonthlyDayNo { get; set; }

        [Required]
        [Display(Name = "Monthly frequency")]
        [Range(typeof(int), "1", "99", ErrorMessage = "Number must be between 1 and 99.")]
        public int MonthlyFrequency { get; set; }


        // Monthly Relative Schedules
        [Required]
        [Display(Name = "Monthly frequency")]
        [Range(typeof(int), "1", "99", ErrorMessage = "Number must be between 1 and 99.")]
        public int MonthlyRelativeFreq { get; set; }

        public string MonthlyRelativeFreqSubDayType { get; set; }

        public string MonthlyRelativeSubFreq { get; set; }



        // Daily Frequency for Daily, Weekly and Monthly
        public bool DailyFreqOccursOnce { get; set; }

        [Required]
        [Display(Name = "Daily start time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan DailyFreqOccursOnceTime { get; set; }

        [Required]
        [Display(Name = "Daily occurrance")]
        [Range(typeof(int), "1", "100", ErrorMessage = "Number must be between 1 and 100.")]
        public int DailyFreqOccursEvery { get; set; }
        public string DailyFreqSubDay { get; set; }

        [Required]
        [Display(Name = "Daily start time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan DailyFreqStartingTime { get; set; }

        [Required]
        [Display(Name = "Daily end time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan DailyFreqEndingTime { get; set; }


        // duration for Daily, Weekly and Monthly
        [Required]
        [Display(Name = "Schedule start date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DurationStartDate { get; set; }

        [Required]
        [Display(Name = "Schedule end date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DurationEndDate { get; set; }
        public bool DurationNoEndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ScheduleFrequency == "OneTime" && OneTimeStartDate.Date < DateTime.Now.Date)
            {
                yield return new ValidationResult("Start Date cannot be before today.");
            }

            if (ScheduleFrequency == "Weekly" 
                && (!WeeklySunday || !WeeklyMonday || !WeeklyTuesday || !WeeklyWednesday || !WeeklyThursday || !WeeklyFriday || !WeeklySaturday))
            {
                yield return new ValidationResult("At least one day must be selected.");
            }
        }
    }
}