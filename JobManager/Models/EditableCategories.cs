using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace JobManager.Models
{
    public class EditableCategories
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public bool Editable { get; set; }
    }
}