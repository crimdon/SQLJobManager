using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace JobManager.Models
{
    public class ActivityLog
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string UserName { get; set; }
        public string ServerName { get; set; }
        public string JobName { get; set; }
        public string Action { get; set; }
    }
}