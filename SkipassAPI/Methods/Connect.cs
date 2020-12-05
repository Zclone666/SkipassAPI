using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Methods
{
    public static class Connect
    {
        public static bool Test()
        {
            bool ret = false;

            using (SqlConnection conn = new SqlConnection(Const.Paths.SQLPath))
            {
                conn.Open();
                using(SqlCommand cmd=new SqlCommand(@"SELECT AccountStockId FROM [Ski2Db_2015-2016].[dbo].[AccountStock_test]", conn))
                {
                    try
                    {
                        string res="";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res += reader[0].ToString();
                            }
                        }
                        if (res!="") ret = true;
                    }catch(Exception e) { return ret; }
                }
                conn.Close();
            }

            return ret;
        }
    }
}
