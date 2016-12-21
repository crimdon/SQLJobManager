using System;
using Microsoft.SqlServer.Management.Smo.Agent;
using System.ComponentModel.DataAnnotations;

namespace JobManager.Models
{
    public class StepDetailsModel
    {
        public string ServerName { get; set; }
        public Guid JobID { get; set; }
        public int StepNo { get; set; }
        [Required]
        public string StepName { get; set; }
        public string RunAs { get; set; }
        [Required]
        public string Database { get; set; }
        [Required]
        public string Command { get; set; }
        [Required]
        public string OnSuccess { get; set; }
        [Required]
        public string OnFailure { get; set; }

    }
}