using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobManager.Models;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using System.Data;
using System.Collections;
using System.Globalization;

namespace JobManager.Helpers
{
    public class JobSchedules
    {
        public List<JobScheduleListModel> getSchedules(string serverName, Guid jobID)
        {
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(serverName);
            Job job = dbServer.JobServer.GetJobByID(jobID);

            List<JobScheduleListModel> schedulelist = new List<JobScheduleListModel>();

            string sql = @"SELECT msdb.dbo.sysjobs.job_id AS [JobID]
                             , msdb.dbo.sysjobschedules.schedule_id AS [ScheduleID]
                             , msdb.dbo.sysschedules.schedule_uid AS [ScheduleUID]
                             , msdb.dbo.sysschedules.name AS [ScheduleName]
                             , CASE
                                   WHEN msdb.dbo.sysjobs.job_id IS NULL THEN 'Unscheduled'
                                   WHEN msdb.dbo.sysschedules.freq_type = 0x1 -- OneTime
                                       THEN
                                           'Once on '
                                         + CONVERT(
                                                      CHAR(10)
                                                    , CAST( CAST( msdb.dbo.sysschedules.active_start_date AS VARCHAR ) AS DATETIME )
                                                    , 102 -- yyyy.mm.dd
                                                   )
                                   WHEN msdb.dbo.sysschedules.freq_type = 0x4 -- Daily
                                       THEN 'Daily'
                                   WHEN msdb.dbo.sysschedules.freq_type = 0x8 -- weekly
                                       THEN
                                           CASE
                                               WHEN msdb.dbo.sysschedules.freq_recurrence_factor = 1
                                                   THEN 'Weekly on '
                                               WHEN msdb.dbo.sysschedules.freq_recurrence_factor > 1
                                                   THEN 'Every '
                                                      + CAST( msdb.dbo.sysschedules.freq_recurrence_factor AS VARCHAR )
                                                      + ' weeks on '
                                           END
                                         + LEFT(
                                                     CASE WHEN msdb.dbo.sysschedules.freq_interval &  1 =  1 THEN 'Sunday, '    ELSE '' END
                                                   + CASE WHEN msdb.dbo.sysschedules.freq_interval &  2 =  2 THEN 'Monday, '    ELSE '' END
                                                   + CASE WHEN msdb.dbo.sysschedules.freq_interval &  4 =  4 THEN 'Tuesday, '   ELSE '' END
                                                   + CASE WHEN msdb.dbo.sysschedules.freq_interval &  8 =  8 THEN 'Wednesday, ' ELSE '' END
                                                   + CASE WHEN msdb.dbo.sysschedules.freq_interval & 16 = 16 THEN 'Thursday, '  ELSE '' END
                                                   + CASE WHEN msdb.dbo.sysschedules.freq_interval & 32 = 32 THEN 'Friday, '    ELSE '' END
                                                   + CASE WHEN msdb.dbo.sysschedules.freq_interval & 64 = 64 THEN 'Saturday, '  ELSE '' END
                                                 , LEN(
                                                            CASE WHEN msdb.dbo.sysschedules.freq_interval &  1 =  1 THEN 'Sunday, '    ELSE '' END
                                                          + CASE WHEN msdb.dbo.sysschedules.freq_interval &  2 =  2 THEN 'Monday, '    ELSE '' END
                                                          + CASE WHEN msdb.dbo.sysschedules.freq_interval &  4 =  4 THEN 'Tuesday, '   ELSE '' END
                                                          + CASE WHEN msdb.dbo.sysschedules.freq_interval &  8 =  8 THEN 'Wednesday, ' ELSE '' END
                                                          + CASE WHEN msdb.dbo.sysschedules.freq_interval & 16 = 16 THEN 'Thursday, '  ELSE '' END
                                                          + CASE WHEN msdb.dbo.sysschedules.freq_interval & 32 = 32 THEN 'Friday, '    ELSE '' END
                                                          + CASE WHEN msdb.dbo.sysschedules.freq_interval & 64 = 64 THEN 'Saturday, '  ELSE '' END
                                                       )  - 1  -- LEN() ignores trailing spaces
                                               )
                                   WHEN msdb.dbo.sysschedules.freq_type = 0x10 -- monthly
                                       THEN
                                           CASE
                                               WHEN msdb.dbo.sysschedules.freq_recurrence_factor = 1
                                                   THEN 'Monthly on the '
                                               WHEN msdb.dbo.sysschedules.freq_recurrence_factor > 1
                                                   THEN 'Every '
                                                      + CAST( msdb.dbo.sysschedules.freq_recurrence_factor AS VARCHAR )
                                                      + ' months on the '
                                           END
                                         + CAST( msdb.dbo.sysschedules.freq_interval AS VARCHAR )
                                         + CASE
                                               WHEN msdb.dbo.sysschedules.freq_interval IN ( 1, 21, 31 ) THEN 'st'
                                               WHEN msdb.dbo.sysschedules.freq_interval IN ( 2, 22     ) THEN 'nd'
                                               WHEN msdb.dbo.sysschedules.freq_interval IN ( 3, 23     ) THEN 'rd'
                                               ELSE 'th'
                                           END
                                   WHEN msdb.dbo.sysschedules.freq_type = 0x20 -- monthly relative
                                       THEN
                                           CASE
                                               WHEN msdb.dbo.sysschedules.freq_recurrence_factor = 1
                                                   THEN 'Monthly on the '
                                               WHEN msdb.dbo.sysschedules.freq_recurrence_factor > 1
                                                   THEN 'Every '
                                                      + CAST( msdb.dbo.sysschedules.freq_recurrence_factor AS VARCHAR )
                                                      + ' months on the '
                                           END
                                         + CASE msdb.dbo.sysschedules.freq_relative_interval
                                               WHEN 0x01 THEN 'first '
                                               WHEN 0x02 THEN 'second '
                                               WHEN 0x04 THEN 'third '
                                               WHEN 0x08 THEN 'fourth '
                                               WHEN 0x10 THEN 'last '
                                           END
                                         + CASE msdb.dbo.sysschedules.freq_interval
                                               WHEN  1 THEN 'Sunday'
                                               WHEN  2 THEN 'Monday'
                                               WHEN  3 THEN 'Tuesday'
                                               WHEN  4 THEN 'Wednesday'
                                               WHEN  5 THEN 'Thursday'
                                               WHEN  6 THEN 'Friday'
                                               WHEN  7 THEN 'Saturday'
                                               WHEN  8 THEN 'day'
                                               WHEN  9 THEN 'week day'
                                               WHEN 10 THEN 'weekend day'
                                           END
                                   WHEN msdb.dbo.sysschedules.freq_type = 0x40
                                       THEN 'Automatically starts when SQLServerAgent starts.'
                                   WHEN msdb.dbo.sysschedules.freq_type = 0x80
                                       THEN 'Starts whenever the CPUs become idle'
                                   ELSE ''
                               END
                             + CASE
                                   WHEN msdb.dbo.sysjobs.enabled = 0 THEN ''
                                   WHEN msdb.dbo.sysjobs.job_id IS NULL THEN ''
                                   WHEN msdb.dbo.sysschedules.freq_subday_type = 0x1 OR msdb.dbo.sysschedules.freq_type = 0x1
                                       THEN ' at '
                                                    + Case  -- Depends on time being integer to drop right-side digits
                                                            when(msdb.dbo.sysschedules.active_start_time % 1000000)/10000 = 0 then 
                                                                              '12'
                                                                            + ':'  
                                                                            +Replicate('0',2 - len(convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100)))
                                                                            + convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100) 
                                                                            + ' AM'
                                                            when (msdb.dbo.sysschedules.active_start_time % 1000000)/10000< 10 then
                                                                            convert(char(1),(msdb.dbo.sysschedules.active_start_time % 1000000)/10000) 
                                                                            + ':'  
                                                                            +Replicate('0',2 - len(convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100))) 
                                                                            + convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100) 
                                                                            + ' AM'
                                                            when (msdb.dbo.sysschedules.active_start_time % 1000000)/10000 < 12 then
                                                                            convert(char(2),(msdb.dbo.sysschedules.active_start_time % 1000000)/10000) 
                                                                            + ':'  
                                                                            +Replicate('0',2 - len(convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100))) 
                                                                            + convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100) 
                                                                            + ' AM'
                                                            when (msdb.dbo.sysschedules.active_start_time % 1000000)/10000< 22 then
                                                                            convert(char(1),((msdb.dbo.sysschedules.active_start_time % 1000000)/10000) - 12) 
                                                                            + ':'  
                                                                            +Replicate('0',2 - len(convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100))) 
                                                                            + convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100) 
                                                                            + ' PM'
                                                            else        convert(char(2),((msdb.dbo.sysschedules.active_start_time % 1000000)/10000) - 12)
                                                                            + ':'  
                                                                            +Replicate('0',2 - len(convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100))) 
                                                                            + convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100) 
                                                                            + ' PM'
                                                    end
                                   WHEN msdb.dbo.sysschedules.freq_subday_type IN ( 0x2, 0x4, 0x8 )
                                       THEN ' every '
                                         + CAST( msdb.dbo.sysschedules.freq_subday_interval AS VARCHAR )
                                         + CASE freq_subday_type
                                               WHEN 0x2 THEN ' second'
                                               WHEN 0x4 THEN ' minute'
                                               WHEN 0x8 THEN ' hour'
                                           END
                                         + CASE
                                               WHEN msdb.dbo.sysschedules.freq_subday_interval > 1 THEN 's'
                                                               ELSE '' -- Added default 3/21/08; John Arnott
                                           END
                                   ELSE ''
                               END
                             + CASE
                                   WHEN msdb.dbo.sysjobs.enabled = 0 THEN ''
                                   WHEN msdb.dbo.sysjobs.job_id IS NULL THEN ''
                                   WHEN msdb.dbo.sysschedules.freq_subday_type IN ( 0x2, 0x4, 0x8 )
                                       THEN ' between '
                                                    + Case  -- Depends on time being integer to drop right-side digits
                                                            when(msdb.dbo.sysschedules.active_start_time % 1000000)/10000 = 0 then 
                                                                              '12'
                                                                            + ':'  
                                                                            +Replicate('0',2 - len(convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100)))
                                                                            + rtrim(convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100))
                                                                            + ' AM'
                                                            when (msdb.dbo.sysschedules.active_start_time % 1000000)/10000< 10 then
                                                                            convert(char(1),(msdb.dbo.sysschedules.active_start_time % 1000000)/10000) 
                                                                            + ':'  
                                                                            +Replicate('0',2 - len(convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100))) 
                                                                            + rtrim(convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100))
                                                                            + ' AM'
                                                            when (msdb.dbo.sysschedules.active_start_time % 1000000)/10000 < 12 then
                                                                            convert(char(2),(msdb.dbo.sysschedules.active_start_time % 1000000)/10000) 
                                                                            + ':'  
                                                                            +Replicate('0',2 - len(convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100))) 
                                                                            + rtrim(convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100)) 
                                                                            + ' AM'
                                                            when (msdb.dbo.sysschedules.active_start_time % 1000000)/10000< 22 then
                                                                            convert(char(1),((msdb.dbo.sysschedules.active_start_time % 1000000)/10000) - 12) 
                                                                            + ':'  
                                                                            +Replicate('0',2 - len(convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100))) 
                                                                            + rtrim(convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100)) 
                                                                            + ' PM'
                                                            else        convert(char(2),((msdb.dbo.sysschedules.active_start_time % 1000000)/10000) - 12)
                                                                            + ':'  
                                                                            +Replicate('0',2 - len(convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100))) 
                                                                            + rtrim(convert(char(2),(msdb.dbo.sysschedules.active_start_time % 10000)/100))
                                                                            + ' PM'
                                                    end
                                         + ' and '
                                                    + Case  -- Depends on time being integer to drop right-side digits
                                                            when(msdb.dbo.sysschedules.active_end_time % 1000000)/10000 = 0 then 
                                                                            '12'
                                                                            + ':'  
                                                                            +Replicate('0',2 - len(convert(char(2),(msdb.dbo.sysschedules.active_end_time % 10000)/100)))
                                                                            + rtrim(convert(char(2),(msdb.dbo.sysschedules.active_end_time % 10000)/100))
                                                                            + ' AM'
                                                            when (msdb.dbo.sysschedules.active_end_time % 1000000)/10000< 10 then
                                                                            convert(char(1),(msdb.dbo.sysschedules.active_end_time % 1000000)/10000) 
                                                                            + ':'  
                                                                            +Replicate('0',2 - len(convert(char(2),(msdb.dbo.sysschedules.active_end_time % 10000)/100))) 
                                                                            + rtrim(convert(char(2),(msdb.dbo.sysschedules.active_end_time % 10000)/100))
                                                                            + ' AM'
                                                            when (msdb.dbo.sysschedules.active_end_time % 1000000)/10000 < 12 then
                                                                            convert(char(2),(msdb.dbo.sysschedules.active_end_time % 1000000)/10000) 
                                                                            + ':'  
                                                                            +Replicate('0',2 - len(convert(char(2),(msdb.dbo.sysschedules.active_end_time % 10000)/100))) 
                                                                            + rtrim(convert(char(2),(msdb.dbo.sysschedules.active_end_time % 10000)/100))
                                                                            + ' AM'
                                                            when (msdb.dbo.sysschedules.active_end_time % 1000000)/10000< 22 then
                                                                            convert(char(1),((msdb.dbo.sysschedules.active_end_time % 1000000)/10000) - 12)
                                                                            + ':'  
                                                                            +Replicate('0',2 - len(convert(char(2),(msdb.dbo.sysschedules.active_end_time % 10000)/100))) 
                                                                            + rtrim(convert(char(2),(msdb.dbo.sysschedules.active_end_time % 10000)/100)) 
                                                                            + ' PM'
                                                            else        convert(char(2),((msdb.dbo.sysschedules.active_end_time % 1000000)/10000) - 12)
                                                                            + ':'  
                                                                            +Replicate('0',2 - len(convert(char(2),(msdb.dbo.sysschedules.active_end_time % 10000)/100))) 
                                                                            + rtrim(convert(char(2),(msdb.dbo.sysschedules.active_end_time % 10000)/100)) 
                                                                            + ' PM'
                                                    end
                                   ELSE ''
                               END AS Schedule,
                            CASE WHEN msdb.dbo.sysschedules.enabled = 1 THEN 'True'
                                 WHEN msdb.dbo.sysschedules.enabled = 0 THEN 'False' END AS [Schedule Enabled]

                            FROM         msdb.dbo.sysjobs INNER JOIN
                                                  msdb.dbo.sysjobschedules ON msdb.dbo.sysjobs.job_id = msdb.dbo.sysjobschedules.job_id INNER JOIN
                                                  msdb.dbo.sysschedules ON msdb.dbo.sysjobschedules.schedule_id = msdb.dbo.sysschedules.schedule_id
                            where msdb.dbo.sysjobs.job_id = '" + jobID + @"'
                            order by 2
                            ";

            DataSet ds = dbServer.ConnectionContext.ExecuteWithResults(sql);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                schedulelist.Add(new JobScheduleListModel
                {
                    ScheduleID = int.Parse(row["ScheduleID"].ToString()),
                    ScheduleUID = Guid.Parse(row["ScheduleUID"].ToString()),
                    Name = row["ScheduleName"].ToString(),
                    Enabled = bool.Parse(row["Schedule Enabled"].ToString()),
                    Description = row["Schedule"].ToString()
                });
            }
            return schedulelist;
        }

        public void deleteSchedule(string serverName, Guid jobID, Guid scheduleUID)
        {
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(serverName);
            Job job = dbServer.JobServer.GetJobByID(jobID);

            JobSchedule schedule = job.JobSchedules[scheduleUID];

            job.JobSchedules[scheduleUID].Drop();
            job.JobSchedules.Refresh();
        }

        public JobScheduleDetailsModel getScheduleDetails (string serverName, Guid jobID, Guid scheduleUID)
        {
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(serverName);
            JobScheduleDetailsModel schedule = new JobScheduleDetailsModel();
            Job job = dbServer.JobServer.GetJobByID(jobID);

            var jobschedule = job.JobSchedules[scheduleUID];

            schedule.IsEnabled = jobschedule.IsEnabled;
            schedule.Name = jobschedule.Name;
            schedule.ScheduleFrequency = jobschedule.FrequencyTypes.ToString();
            schedule.ServerName = serverName;
            schedule.JobID = jobID;
            schedule.ScheduleUID = scheduleUID;
            schedule = setDefaults(schedule);

            switch (jobschedule.FrequencyTypes.ToString())
            {
                case "OneTime":
                    schedule.OneTimeStartDate = jobschedule.ActiveStartDate;
                    schedule.OneTimeStartTimeOfDay = jobschedule.ActiveStartTimeOfDay;
                    break;

                case "Daily":
                    schedule.DailyRecursEvery = jobschedule.FrequencyInterval;
                    break;

                case "Weekly":
                    WeekDays days = (WeekDays)jobschedule.FrequencyInterval;
                    schedule.WeeklyRecursEvery = jobschedule.FrequencyRecurrenceFactor;
                    if (days.HasFlag(WeekDays.Sunday))
                        schedule.WeeklySunday = true;
                    if (days.HasFlag(WeekDays.Monday))
                        schedule.WeeklyMonday = true;
                    if (days.HasFlag(WeekDays.Tuesday))
                        schedule.WeeklyTuesday = true;
                    if (days.HasFlag(WeekDays.Wednesday))
                        schedule.WeeklyWednesday = true;
                    if (days.HasFlag(WeekDays.Thursday))
                        schedule.WeeklyThursday = true;
                    if (days.HasFlag(WeekDays.Friday))
                        schedule.WeeklyFriday = true;
                    if (days.HasFlag(WeekDays.Saturday))
                        schedule.WeeklySaturday = true;
                    break;

                case "Monthly":
                    schedule.MonthlyDayNo = jobschedule.FrequencyInterval;
                    schedule.MonthlyFrequency = jobschedule.FrequencyRecurrenceFactor;
                    break;

                case "MonthlyRelative":
                    MonthlyRelativeWeekDays monthdays = (MonthlyRelativeWeekDays)jobschedule.FrequencyInterval;
                    schedule.MonthlyRelativeFreq = jobschedule.FrequencyRecurrenceFactor;
                    schedule.MonthlyRelativeFreqSubDayType = jobschedule.FrequencyRelativeIntervals.ToString();
                    if (monthdays.HasFlag(MonthlyRelativeWeekDays.Sunday))
                        schedule.MonthlyRelativeSubFreq = "Sunday";
                    if (monthdays.HasFlag(MonthlyRelativeWeekDays.Monday))
                        schedule.MonthlyRelativeSubFreq = "Monday";
                    if (monthdays.HasFlag(MonthlyRelativeWeekDays.Tuesday))
                        schedule.MonthlyRelativeSubFreq = "Tuesday";
                    if (monthdays.HasFlag(MonthlyRelativeWeekDays.Wednesday))
                        schedule.MonthlyRelativeSubFreq = "Wednesday";
                    if (monthdays.HasFlag(MonthlyRelativeWeekDays.Thursday))
                        schedule.MonthlyRelativeSubFreq = "Thursday";
                    if (monthdays.HasFlag(MonthlyRelativeWeekDays.Friday))
                        schedule.MonthlyRelativeSubFreq = "Friday";
                    if (monthdays.HasFlag(MonthlyRelativeWeekDays.Saturday))
                        schedule.MonthlyRelativeSubFreq = "Saturday";
                    if (monthdays.HasFlag(MonthlyRelativeWeekDays.WeekDays))
                        schedule.MonthlyRelativeSubFreq = "WeekDays";
                    if (monthdays.HasFlag(MonthlyRelativeWeekDays.WeekEnds))
                        schedule.MonthlyRelativeSubFreq = "WeekEnds";
                    if (monthdays.HasFlag(MonthlyRelativeWeekDays.EveryDay))
                        schedule.MonthlyRelativeSubFreq = "EveryDay";
                    break;
                
                default:
                    schedule.OneTimeStartDate = jobschedule.ActiveStartDate;
                    schedule.OneTimeStartTimeOfDay = jobschedule.ActiveStartTimeOfDay;
                    break;
            }

            if (jobschedule.ActiveEndTimeOfDay.Hours == 23 && jobschedule.ActiveEndTimeOfDay.Minutes == 59 && jobschedule.ActiveEndTimeOfDay.Seconds == 59)
            {
                schedule.DailyFreqOccursOnce = true;
                schedule.DailyFreqOccursOnceTime = jobschedule.ActiveStartTimeOfDay;
            }
            else
            {
                schedule.DailyFreqOccursOnce = false;
                schedule.DailyFreqOccursEvery = jobschedule.FrequencySubDayInterval;
                schedule.DailyFreqSubDay = jobschedule.FrequencySubDayTypes.ToString();
                schedule.DailyFreqStartingTime = jobschedule.ActiveStartTimeOfDay;
                schedule.DailyFreqEndingTime = jobschedule.ActiveEndTimeOfDay;
            }
            schedule.DurationStartDate = jobschedule.ActiveStartDate;
            schedule.DurationEndDate = jobschedule.ActiveEndDate;
            if (jobschedule.ActiveEndDate == DateTime.MaxValue.Date)
                schedule.DurationNoEndDate = true;

            return schedule;
        }

        public void saveScheduleDetails(JobScheduleDetailsModel schedule)
        {
            ConnectSqlServer connection = new ConnectSqlServer();
            Server dbServer = connection.Connect(schedule.ServerName);
            Job job = dbServer.JobServer.GetJobByID(schedule.JobID);
            JobSchedule scheduleToUpdate = job.JobSchedules[schedule.ScheduleUID];

            if (schedule.Name != scheduleToUpdate.Name)
            {
                scheduleToUpdate.Rename(schedule.Name);
                scheduleToUpdate.Refresh();
            }

            scheduleToUpdate.IsEnabled = schedule.IsEnabled;

            switch(schedule.ScheduleFrequency)
            {
                case "OneTime":
                    scheduleToUpdate.FrequencyTypes = FrequencyTypes.OneTime;
                    scheduleToUpdate.ActiveStartDate = schedule.OneTimeStartDate;
                    scheduleToUpdate.ActiveStartTimeOfDay = schedule.OneTimeStartTimeOfDay;
                    break;

                case "Daily":
                    scheduleToUpdate.FrequencyTypes = FrequencyTypes.Daily;
                    scheduleToUpdate.FrequencyInterval = schedule.DailyRecursEvery;
                    scheduleToUpdate = setCommon(scheduleToUpdate, schedule);
                    break;

                case "Weekly":
                    scheduleToUpdate.FrequencyTypes = FrequencyTypes.Weekly;
                    scheduleToUpdate.FrequencyRecurrenceFactor = schedule.WeeklyRecursEvery;
                    scheduleToUpdate.FrequencyInterval = 0;
                    if (schedule.WeeklySunday)
                        scheduleToUpdate.FrequencyInterval = +1;
                    if (schedule.WeeklyMonday)
                        scheduleToUpdate.FrequencyInterval = +2;
                    if (schedule.WeeklyTuesday)
                        scheduleToUpdate.FrequencyInterval = +4;
                    if (schedule.WeeklyWednesday)
                        scheduleToUpdate.FrequencyInterval = +8;
                    if (schedule.WeeklyThursday)
                        scheduleToUpdate.FrequencyInterval = +16;
                    if (schedule.WeeklyFriday)
                        scheduleToUpdate.FrequencyInterval = +32;
                    if (schedule.WeeklySaturday)
                        scheduleToUpdate.FrequencyInterval = +64;
                    scheduleToUpdate = setCommon(scheduleToUpdate, schedule);
                    break;

                case "Monthly":
                    scheduleToUpdate.FrequencyTypes = FrequencyTypes.Monthly;
                    scheduleToUpdate.FrequencyRecurrenceFactor = schedule.MonthlyFrequency;
                    scheduleToUpdate.FrequencyInterval = schedule.MonthlyDayNo;
                    scheduleToUpdate = setCommon(scheduleToUpdate, schedule);
                    break;

                case "MonthlyRelative":
                    scheduleToUpdate.FrequencyTypes = FrequencyTypes.MonthlyRelative;
                    if (schedule.MonthlyRelativeSubFreq == "Sunday")
                        scheduleToUpdate.FrequencyInterval = 1;
                    if (schedule.MonthlyRelativeSubFreq == "Monday")
                        scheduleToUpdate.FrequencyInterval = 2;
                    if (schedule.MonthlyRelativeSubFreq == "Tuesday")
                        scheduleToUpdate.FrequencyInterval = 3;
                    if (schedule.MonthlyRelativeSubFreq == "Wednesday")
                        scheduleToUpdate.FrequencyInterval = 4;
                    if (schedule.MonthlyRelativeSubFreq == "Thursday")
                        scheduleToUpdate.FrequencyInterval = 5;
                    if (schedule.MonthlyRelativeSubFreq == "Friday")
                        scheduleToUpdate.FrequencyInterval = 6;
                    if (schedule.MonthlyRelativeSubFreq == "Saturday")
                        scheduleToUpdate.FrequencyInterval = 7;
                    if (schedule.MonthlyRelativeSubFreq == "Weekdays")
                        scheduleToUpdate.FrequencyInterval = 9;
                    if (schedule.MonthlyRelativeSubFreq == "WeekEnds")
                        scheduleToUpdate.FrequencyInterval = 10;
                    if (schedule.MonthlyRelativeSubFreq == "EveryDay")
                        scheduleToUpdate.FrequencyInterval = 8;
                    switch(schedule.MonthlyRelativeSubFreq)
                    {
                        case "First":
                            scheduleToUpdate.FrequencyRelativeIntervals = FrequencyRelativeIntervals.First;
                            break;
                        case "Second":
                            scheduleToUpdate.FrequencyRelativeIntervals = FrequencyRelativeIntervals.Second;
                            break;
                        case "Third":
                            scheduleToUpdate.FrequencyRelativeIntervals = FrequencyRelativeIntervals.Third;
                            break;
                        case "Fourth":
                            scheduleToUpdate.FrequencyRelativeIntervals = FrequencyRelativeIntervals.Fourth;
                            break;
                        case "Last":
                            scheduleToUpdate.FrequencyRelativeIntervals = FrequencyRelativeIntervals.Last;
                            break;
                    }
                    scheduleToUpdate.FrequencyRecurrenceFactor = schedule.MonthlyRelativeFreq;
                    scheduleToUpdate = setCommon(scheduleToUpdate, schedule);
                    break;

                case "AutoStart":
                    scheduleToUpdate.FrequencyTypes = FrequencyTypes.AutoStart;
                    scheduleToUpdate.FrequencyInterval = 0;
                    scheduleToUpdate.FrequencyRecurrenceFactor = 0;
                    scheduleToUpdate.FrequencyRelativeIntervals = 0;
                    scheduleToUpdate.FrequencySubDayInterval = 0;
                    scheduleToUpdate.FrequencySubDayTypes = FrequencySubDayTypes.Unknown;
                    break;

                case "OnIdle":
                    scheduleToUpdate.FrequencyTypes = FrequencyTypes.OneTime;
                    scheduleToUpdate.FrequencyInterval = 0;
                    scheduleToUpdate.FrequencyRecurrenceFactor = 0;
                    scheduleToUpdate.FrequencyRelativeIntervals = 0;
                    scheduleToUpdate.FrequencySubDayInterval = 0;
                    scheduleToUpdate.FrequencySubDayTypes = FrequencySubDayTypes.Unknown;
                    break;

            }

            scheduleToUpdate.Alter();
            scheduleToUpdate.Refresh();
        }

        private JobSchedule setCommon (JobSchedule scheduleToUpdate, JobScheduleDetailsModel schedule)
        {
            scheduleToUpdate.ActiveStartTimeOfDay = schedule.DailyFreqOccursOnceTime;
            if (schedule.DailyFreqOccursOnce)
            {
                scheduleToUpdate.ActiveEndTimeOfDay = TimeSpan.ParseExact("23:59:59", "hh\\:mm\\:ss", CultureInfo.InvariantCulture);
                scheduleToUpdate.FrequencySubDayTypes = FrequencySubDayTypes.Once;
            }
            else
            {
                scheduleToUpdate.ActiveEndTimeOfDay = schedule.DailyFreqEndingTime;
                scheduleToUpdate.FrequencySubDayInterval = schedule.DailyFreqOccursEvery;
                switch (schedule.DailyFreqSubDay)
                {
                    case "Hour":
                        scheduleToUpdate.FrequencySubDayTypes = FrequencySubDayTypes.Hour;
                        break;
                    case "Minute":
                        scheduleToUpdate.FrequencySubDayTypes = FrequencySubDayTypes.Minute;
                        break;
                    case "Second":
                        scheduleToUpdate.FrequencySubDayTypes = FrequencySubDayTypes.Second;
                        break;
                }
            }
            scheduleToUpdate.ActiveStartDate = schedule.DurationStartDate;
            if (schedule.DurationNoEndDate)
                scheduleToUpdate.ActiveEndDate = DateTime.MaxValue;
            else
                scheduleToUpdate.ActiveEndDate = schedule.DurationEndDate;

            return scheduleToUpdate;
        }

        private JobScheduleDetailsModel setDefaults (JobScheduleDetailsModel schedule)
        {
            schedule.OneTimeStartDate = DateTime.Now.Date;
            schedule.OneTimeStartTimeOfDay = TimeSpan.MinValue;
            schedule.DailyRecursEvery = 1;
            schedule.WeeklyRecursEvery = 1;
            schedule.MonthlyDayNo = 1;
            schedule.MonthlyFrequency = 1;
            schedule.MonthlyRelativeFreq = 1;
            schedule.MonthlyRelativeFreqSubDayType = "First";
            schedule.MonthlyRelativeSubFreq = "EveryDay";
            schedule.DailyFreqOccursOnce = true;
            schedule.DailyFreqOccursOnceTime = TimeSpan.MinValue;
            schedule.DailyFreqOccursEvery = 1;
            schedule.DailyFreqSubDay = "hour";
            schedule.DailyFreqStartingTime = TimeSpan.MinValue;
            schedule.DailyFreqEndingTime = TimeSpan.ParseExact("23:59:59", "hh\\:mm\\:ss", CultureInfo.InvariantCulture);
            schedule.DurationStartDate = DateTime.Now.Date;
            schedule.DurationEndDate = DateTime.MaxValue.Date;
            schedule.DurationNoEndDate = true;

            return schedule;
        }
    }
}