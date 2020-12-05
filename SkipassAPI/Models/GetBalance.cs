using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Models
{
    public class GetBalanceIn:Alarm
    {
        public string key { get; set; }
    }

    public class GetBalanceOut:Alarm
    {
        public int id { get; set; }
        public string key { get; set; }
        public decimal balance { get; set; }
    }
}
