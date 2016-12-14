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
        [DisplayName("End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime ActiveEndDate { get; set; }
        public TimeSpan ActiveEndTimeOfDay { get; set; }
        [DisplayName("Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime ActiveStartDate { get; set; }
        public TimeSpan ActiveStartTimeOfDay { get; set; }
        public int FrequencyInterval { get; set; }
        public int FrequencyRecurrenceFactor { get; set; }
        public string FrequencyRelativeIntervals { get; set; }
        public int FrequencySubDayInterval { get; set; }
        public string FrequencySubDayTypes { get; set; }
        public string FrequencyTypes { get; set; }
        public bool IsEnabled { get; set; }
        public string Name { get; set; }
        public int MonthlyInterval { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
    }
}