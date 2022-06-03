using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Models
{
    public class User:Alarm
    {
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }

        public bool founded { get; set; }
    }
}
