namespace JobManager.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ActivityLog")]
    public partial class ActivityLog
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
