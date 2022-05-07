using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Models
{
    public class GetBalanceIn:Alarm
    {
        /// <summary>
        /// ID скипасса или браслета
        /// </summary>
        public string key { get; set; }
    }

    public class GetBalanceOut:Alarm
    {
        public int id { get; set; }
        /// <summary>
        /// ID скипасса или браслета
        /// </summary>
        public string key { get; set; }
        public decimal balance { get; set; }
    }
}
