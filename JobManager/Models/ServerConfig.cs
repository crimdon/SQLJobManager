namespace JobManager.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;

    public class ServerConfig
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ServerName { get; set; }
        [Required]
        public AuthenticationType? AuthenticationType { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public enum AuthenticationType
    {
        SQL, Windows
    }
}