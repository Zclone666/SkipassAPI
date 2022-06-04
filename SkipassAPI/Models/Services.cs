using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Models
{
    public class ListServ : Alarm
    {
        public List<Services> services { get; set; } = new List<Services>();
    }

    public class ListServWPrice : Alarm
    {
        public List<ServicesWPrice> services { get; set; } = new List<ServicesWPrice>();
    }

    public class Services
    {
        public int categoryID { get; set; }
        public int stockType { get; set; }
        public string name { get; set; }
    }

    public class AddService
    {
        public int accountStockId { get; set; }
        public int categoryID { get; set; }
        public decimal amount { get; set; } = 1;
        /// <summary>
        /// ID скипасса или браслета
        /// </summary>
        public string key { get; set; }
        public long date_start { get; set; } = DateTime.Now.Ticks;
        public long date_end { get; set; } = DateTime.MaxValue.Ticks;
    }

    public class AddServiceReq:Alarm
    {
        public int accountStockId { get; set; }
        public int categoryID { get; set; }
        public decimal amount { get; set; } = 1;
        /// <summary>
        /// ID скипасса или браслета
        /// </summary>
        public string key { get; set; }
        public long date_start { get; set; } = DateTime.Now.Ticks;
        public long date_end { get; set; } = DateTime.MaxValue.Ticks;
    }
    
    public class AddServiceResp : Alarm
    {
        public AddServiceReq service { get; set; } = new AddServiceReq();
        public bool isSuccess { get; set; }
    }

    public class ServicesWPrice : Services
    {
        public double price { get; set; }
        public string dayT { get; set; }
        public int dayTypeId { get; set; }
    }

    public class ListServWPriceResp
    {
        public Error errors { get; set; } = new Error();
        public List<BaseServResp> services { get; set; } = new List<BaseServResp>();
    }


    public class BaseServResp
    {
        public int categoryID { get; set; }
        public int stockType { get; set; }
        public string name { get; set; }
        public List<PriceResp> price { get; set; } = new List<PriceResp>();
    }

    public class PriceResp
    {
        public double price { get; set; }
        public string dayT { get; set; }
        public int dayTypeId { get; set; }
    }
}
