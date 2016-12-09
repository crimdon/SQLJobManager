using System;
using Microsoft.SqlServer.Management.Smo.Agent;

namespace JobManager.Models
{
    public class StepDetailsModel
    {
        public string ServerName { get; set; }
        public Guid JobID { get; set; }
        public int StepNo { get; set; }
        public string StepName { get; set; }
        public string RunAs { get; set; }
        public string Database { get; set; }
        public string Command { get; set; }
        public string OnSuccess { get; set; }
        public string OnFailure { get; set; }

    }
}