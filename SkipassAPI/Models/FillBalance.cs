﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Models
{
    public class FillBalanceIn : Alarm
    {
        public string key { get; set; }
        public decimal add_sum { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string payment_id { get; set; }
        public string payment_system { get; set; }
        public string payment_source { get; set; }
        public string comment { get; set; }
        public int successed { get; set; }
    }

    public class KeyFound : Alarm
    {
        public bool Founded { get; set; } = false;
    }

}