using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Models
{
    public class Alarm
    {
        public Error errors { get; set; }
        /// <summary>
        /// Ключ авторизации = mn5tq8ZTJSmLA6FJ
        /// </summary>
        public string authkey { get; set; }
    }

    public class Error
    {
        public int code { get; set; } = 0;
        public string message { get; set; }
    }
}

