using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Models
{
    public class User:Alarm
    {

        public UserInfo userInfo { get; set; } = new UserInfo();
        public bool founded { get; set; }
    }

    public class UserInfo
    {
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string key { get; set; }
        public bool isActive { get; set; }
        public decimal balance { get; set; }
    }

    public class UserInfoList:Alarm
    {
        public List<UserInfo> userInfo { get; set; } = new List<UserInfo>();
        public bool founded { get; set; }
    }
}
