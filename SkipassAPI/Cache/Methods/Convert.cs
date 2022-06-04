using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Cache.Methods
{
    public static class Convert
    {
        public static Models.ListServWPriceResp OldToNew(List<Models.ServicesWPrice> old)
        {
            Models.ListServWPriceResp ret = new Models.ListServWPriceResp();
            try
            {
                var tmpsrv = new List<Models.BaseServResp>();

                foreach (var i in Cache.Static.ServiceCache.ServicesWPrice.Select(x => x.categoryID).Distinct())
                {
                    var tmpprices = new List<Models.PriceResp>();
                    foreach (var p in Cache.Static.ServiceCache.ServicesWPrice.Where(x => x.categoryID == i))
                    {
                        tmpprices.Add(new Models.PriceResp() { dayT = p.dayT, dayTypeId = p.dayTypeId, price = p.price });
                    }
                    tmpsrv.Add(new Models.BaseServResp() { categoryID = i, name = Cache.Static.ServiceCache.ServicesWPrice.Where(x => x.categoryID == i).Select(x => x.name).FirstOrDefault(), stockType = Cache.Static.ServiceCache.ServicesWPrice.Where(x => x.categoryID == i).Select(x => x.stockType).FirstOrDefault(), price = tmpprices });
                }
                ret.services = tmpsrv;
            }catch(Exception e)
            {
                ret.errors = new Models.Error() { code = 500, message = e.Message };
                return ret;
            }
            ret.errors = new Models.Error() { code = 0, message = String.Empty };
            return ret;
        }
    }
}
