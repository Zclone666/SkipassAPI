using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Const
{
    public static class Paths
    {
        const string Winter = "Ski2Db_2015-2016";
        const string Summer = "Fwp";
        public static string SQLPath = $"Data Source=176.107.244.8;Initial Catalog={DataBaseName()};Persist Security Info=True;User ID=sa;Password=M169654us"; //"Data Source=176.107.244.8;Initial Catalog=Ski2Db_2015-2016;Persist Security Info=True;User ID=sa;Password=M169654us";
        public static string LocalSQLPath = $"Data Source=SRVMAIN\\SQLEXPRESS;Initial Catalog={DataBaseName()};Persist Security Info=True;User ID=sa;Password=M169654us"; //"Data Source=SRVMAIN\\SQLEXPRESS;Initial Catalog=Ski2Db_2015-2016;Persist Security Info=True;User ID=sa;Password=M169654us";
        static string DataBaseName()
        {
            if (DateTime.Today.Month < 4 && DateTime.Today.Month > 9) return Winter;
            else return Summer;
        }
    }

}
