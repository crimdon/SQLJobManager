﻿using JobManager.Models;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        public int addStep (string serverName, Guid jobID)
        {
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(serverName);
            Job job = dbServer.JobServer.GetJobByID(jobID);

            JobStep step = new JobStep(job,"New Step");
            step.SubSystem = AgentSubSystem.TransactSql;
            step.OnSuccessAction = StepCompletionAction.QuitWithSuccess;
            step.OnFailAction = StepCompletionAction.QuitWithFailure;
            step.Create();
            step.Refresh();
            return step.ID;

        }

        public void deleteStep(string serverName, Guid jobID, int stepID)
        {
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(serverName);
            Job job = dbServer.JobServer.GetJobByID(jobID);

            job.JobSteps[stepID - 1].Drop();
            job.JobSteps.Refresh();
        }

        public JobStepDetailsModel getStepDetails(string serverName, Guid jobID, int stepID)
        {
            JobStepDetailsModel step = new JobStepDetailsModel();
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(serverName);
            Job job = dbServer.JobServer.GetJobByID(jobID);
            JobStep jobstep = job.JobSteps[stepID - 1];

            step.ServerName = serverName;
            step.JobID = jobID;
            step.StepNo = jobstep.ID;
            step.StepName = jobstep.Name;
            step.RunAs = jobstep.ProxyName;
            step.Database = jobstep.DatabaseName;
            step.Command = jobstep.Command;
            switch (jobstep.OnSuccessAction)
            {
                case StepCompletionAction.GoToNextStep:
                    step.OnSuccess = "GoToNextStep";
                    break;
                case StepCompletionAction.QuitWithSuccess:
                    step.OnSuccess = "QuitWithSuccess";
                    break;
                case StepCompletionAction.QuitWithFailure:
                    step.OnSuccess = "QuitWithFailure";
                    break;
                case StepCompletionAction.GoToStep:
                    step.OnSuccess = "GoToStep:" + jobstep.OnSuccessStep;
                    break;
            }
            switch (jobstep.OnFailAction)
            {
                case StepCompletionAction.GoToNextStep:
                    step.OnFailure = "GoToNextStep";
                    break;
                case StepCompletionAction.QuitWithSuccess:
                    step.OnFailure = "QuitWithSuccess";
                    break;
                case StepCompletionAction.QuitWithFailure:
                    step.OnFailure = "QuitWithFailure";
                    break;
                case StepCompletionAction.GoToStep:
                    step.OnFailure = "GoToStep:" + jobstep.OnFailStep;
                    break;
            }

            return step;
        }

        public void saveStepDetails(JobStepDetailsModel Step)
        {
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(Step.ServerName);
            Job job = dbServer.JobServer.GetJobByID(Step.JobID);

            JobStep stepToUpdate = job.JobSteps[Step.StepNo - 1];

            if (stepToUpdate.Name != Step.StepName)
                stepToUpdate.Rename(Step.StepName);
            stepToUpdate.DatabaseName = Step.Database;
            stepToUpdate.Command = Step.Command;
            switch (Step.OnSuccess)
            {
                case "GoToNextStep":
                    stepToUpdate.OnSuccessAction = StepCompletionAction.GoToNextStep;
                    stepToUpdate.OnSuccessStep = 0;
                    break;
                case "QuitWithSuccess":
                    stepToUpdate.OnSuccessAction = StepCompletionAction.QuitWithSuccess;
                    stepToUpdate.OnSuccessStep = 0;
                    break;
                case "QuitWithFailure":
                    stepToUpdate.OnSuccessAction = StepCompletionAction.QuitWithFailure;
                    stepToUpdate.OnSuccessStep = 0;
                    break;
                default:
                    stepToUpdate.OnSuccessAction = StepCompletionAction.GoToStep;
                    stepToUpdate.OnSuccessStep = int.Parse(Step.OnSuccess.Split(':')[1]);
                    break;
            }
            switch (Step.OnFailure)
            {
                case "GoToNextStep":
                    stepToUpdate.OnFailAction = StepCompletionAction.GoToNextStep;
                    stepToUpdate.OnFailStep = 0;
                    break;
                case "QuitWithSuccess":
                    stepToUpdate.OnFailAction = StepCompletionAction.QuitWithSuccess;
                    stepToUpdate.OnFailStep = 0;
                    break;
                case "QuitWithFailure":
                    stepToUpdate.OnFailAction = StepCompletionAction.QuitWithFailure;
                    stepToUpdate.OnFailStep = 0;
                    break;
                default:
                    stepToUpdate.OnFailAction = StepCompletionAction.GoToStep;
                    stepToUpdate.OnFailStep = int.Parse(Step.OnFailure.Split(':')[1]);
                    break;
            }

            stepToUpdate.Alter();
            stepToUpdate.Refresh();
        }

        public void moveStepUp (string serverName, Guid jobID, int stepID)
        {
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(serverName);
            Job job = dbServer.JobServer.GetJobByID(jobID);

            JobStep newJobStep = job.JobSteps[stepID - 1];
            StringCollection script = newJobStep.Script();
            script[0] = script[0].Replace("@step_id=" + stepID, "@step_id=" + (stepID -1));
            job.JobSteps[stepID - 1].Drop();
            dbServer.Refresh();
            dbServer.ConnectionContext.ExecuteNonQuery(script);
        }

        public void moveStepDown(string serverName, Guid jobID, int stepID)
        {
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(serverName);
            Job job = dbServer.JobServer.GetJobByID(jobID);

            JobStep newJobStep = job.JobSteps[stepID - 1];
            StringCollection script = newJobStep.Script();
            script[0] = script[0].Replace("@step_id=" + stepID, "@step_id=" + (stepID + 1));
            job.JobSteps[stepID - 1].Drop();
            dbServer.Refresh();
            dbServer.ConnectionContext.ExecuteNonQuery(script);
        }
    }
}