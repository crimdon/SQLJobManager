using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobManager.Models
{
    public class JobScheduleModel
    {
        public Guid ScheduleID { get; set; }
        public string ScheduleName { get; set; }
        public bool Enabled { get; set; }
        public int FreqType { get; set; }
        public int FreqInterval { get; set; }
        public int FreqSubdayType { get; set; }
        public int FreqSubdayInterval { get; set; }
        public int FreqRelativeInterval { get; set; }
        public int FreqRecurrenceFactor { get; set; }
        public DateTime ActiveStartDate { get; set; }
        public DateTime ActiveEndDate { get; set; }
    }
}