using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Models
{
    public class User:Alarm
    {
        public string firstName { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
    }
}
