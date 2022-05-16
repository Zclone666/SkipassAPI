using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Models
{
    public class UserServicesReq
    {
        public string authkey { get; set; }
        public string key { get; set; }
    }

    public class UserServicesResp:Alarm
    {
        public List<UserServices> services { get; set; } = new List<UserServices>();
    }

    public class UserServices 
    { 
        public string servName { get; set; }
        public bool isActive { get; set; }
        public double amount { get; set; }
        public string start { get; set; }
        public string end { get; set; }

    }

}
