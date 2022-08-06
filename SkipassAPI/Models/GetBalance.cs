using Newtonsoft.Json;
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
        [JsonRequired]
        public string key { get; set; }
    }

    /// <summary>
    /// Класс запроса для получения кода скипасса по email или номеру телефона
    /// </summary>
    public class GetCodeBReq : Alarm
    {
        /// <summary>
        /// Номер телефона в формате +79.......
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// Email пользователя
        /// </summary>
        public string email { get; set; }
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
