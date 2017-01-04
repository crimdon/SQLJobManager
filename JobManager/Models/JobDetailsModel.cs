using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobManager.Models
{
    public class JobDetailsModel
    {
        public Guid JobID { get; set; }
        [Required]
        public string JobName { get; set; }
        [Required]
        public string Owner { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime LastExecuted { get; set; }
        public string ServerName { get; set; }
        public string StepName { get; set; }

    }
}