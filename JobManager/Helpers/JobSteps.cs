using JobManager.Models;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobManager.Helpers
{
    public class JobSteps
    {
        public List<JobStepListModel> getSteps(string serverName, Guid jobID)
        {
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(serverName);
            Job job = dbServer.JobServer.GetJobByID(jobID);

            List<JobStepListModel> steplist = new List<JobStepListModel>();

            foreach (JobStep step in job.JobSteps)
            {
                steplist.Add(new JobStepListModel
                {
                    StepNo = step.ID,
                    StepName = step.Name,
                    StepType = step.SubSystem,
                    OnSuccess = step.OnSuccessAction,
                    OnSuccessStep = step.OnSuccessStep,
                    OnFailure = step.OnFailAction,
                    OnFailureStep = step.OnFailStep
                });
            }
            return steplist;
        }

        public void deleteStep(string serverName, Guid jobID, int stepID)
        {
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(serverName);
            Job job = dbServer.JobServer.GetJobByID(jobID);

            job.JobSteps[stepID - 1].Drop();
            job.JobSteps.Refresh();
        }
    }
}