using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobManager.Models
{
    public class Servers
    {

        public List<string> Serverlist;
        public Servers()
        {
            Serverlist = new List<string>();
            Serverlist.Add("SQLSERVER");
            Serverlist.Add("SQLREPORTS");
        }
    }
}