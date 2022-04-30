using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Models
{
    public class Services:Alarm
    {
        public int CategoryID { get; set; }
        public int StockType { get; set; }
        public string Name { get; set; }
    }
}
