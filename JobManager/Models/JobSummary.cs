using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobManager.Models
{ 
    public class JobSummary
    {
        public Guid JobID { get; set; }
        public string ServerName { get; set; }
        public string JobName { get; set; }
        public bool Enabled { get; set; }
        public string Status { get; set; }
        public string LastOutcome { get; set; }
        public DateTime LastRun { get; set; }
        public DateTime NextRun { get; set; }
        public string Category { get; set; }
        public bool Runable { get; set; }
        public bool Scheduled { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }

    }
}