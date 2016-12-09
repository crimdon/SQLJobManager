﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobManager.Models;

namespace JobManager.Helpers
{
    public class GetConfig
    {
        public static List<IServers> GetServers()
        {
            List<IServers> serverList = new List<IServers>();

            serverList.Add (new ServersModel { ServerName = "SQLSERVER" });
            serverList.Add (new ServersModel { ServerName = "SQLREPORTS" });
            return serverList;
        }

    }
}