namespace JobManager.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ServerConfig")]
    public partial class ServerConfig
    {
        public int Id { get; set; }

        [Required]
        public string ServerName { get; set; }

        public AuthenticationType AuthenticationType { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public enum AuthenticationType
    {
        SQL, Windows
    }
}
