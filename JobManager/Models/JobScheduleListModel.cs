using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobManager.Models
{
    public class JobScheduleListModel
    {
        public int ScheduleID { get; set; }
        public Guid ScheduleUID { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public string Description { get; set; }
    }
}