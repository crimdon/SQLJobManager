using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobManager.Models
{
    public class JobHistoryModel
    {
        public string ServerName { get; set; }
        public Guid JobID { get; set; }
        public string JobName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime ExecutionTime { get; set; }
        public int StepID { get; set; }
        public string StepName { get; set; }
        public string Message { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan Duration { get; set; }
    }

    public class JobHistorySummary
    {
        public string ServerName { get; set; }
        public Guid JobID { get; set; }
        public string JobName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime ExecutionTime { get; set; }
        public int StepID { get; set; }
        public string StepName { get; set; }
        public string Message { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan Duration { get; set; }
        public List<JobHistoryModel> JobHistories { get; set; }
    }
}