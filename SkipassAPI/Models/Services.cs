﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Models
{
    public class Services:Alarm
    {
        public int categoryID { get; set; }
        public int stockType { get; set; }
        public string name { get; set; }
    }

    public class AddService:Alarm
    {
        public int categoryID { get; set; }
        public decimal amount { get; set; }
        /// <summary>
        /// ID скипасса или браслета
        /// </summary>
        public string key { get; set; }
        public long date_start { get; set; } = DateTime.Now.Ticks;
        public long date_end { get; set; } = DateTime.MaxValue.Ticks;
    }
    
    public class AddServiceResp : Alarm
    {
        public AddService service { get; set; }
        public bool isSuccess { get; set; }
    }
}
