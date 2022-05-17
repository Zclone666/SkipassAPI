using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Methods
{
    public static class Global
    {
        public static DateTime UnixTimeToDateTime(long unixtime)
        {
            try
            {
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(unixtime).ToUniversalTime();
                return dtDateTime;
            }
            catch
            {
                return DateTime.Now;
            }
        }
    }
}
