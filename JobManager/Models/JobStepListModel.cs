using System;
using Microsoft.SqlServer.Management.Smo.Agent;

namespace JobManager.Models
{
    public class JobStepListModel
    {
        public int StepNo { get; set; }
        public string StepName { get; set; }
        public AgentSubSystem StepType { get; set; }
        public StepCompletionAction OnSuccess { get; set; }
        public int OnSuccessStep { get; set; }
        public StepCompletionAction OnFailure { get; set; }
        public int OnFailureStep { get; set; }
    }
}