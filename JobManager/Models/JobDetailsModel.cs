using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobManager.Models
{
    public class JobDetailsModel : IValidatableObject
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
        [Required]
        [Display(Name = "Monthly day number")]
        [Range(typeof(int), "1", "99", ErrorMessage = "Invalid step number")]
        public int StartStepID { get; set; }
        public int StepCount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartStepID > StepCount)
            {
                yield return new ValidationResult("Starting step cannot be higher than the number of steps in the job!", new[] { "StartStepID" });
            }
        }
    }
}