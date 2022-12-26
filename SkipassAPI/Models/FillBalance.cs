using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Models
{
    public class FillBalanceIn : Alarm
    {
        /// <summary>
        /// ID скипасса или браслета
        /// </summary>
        public string key { get; set; }
        public decimal add_sum { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string payment_id { get; set; }
        public string payment_system { get; set; }
        public string payment_source { get; set; }
        public string comment { get; set; }
        public int successed { get; set; } = 1;
    }

    public class KeyFound : Alarm
    {
        public bool founded { get; set; } = false;
    }

}
