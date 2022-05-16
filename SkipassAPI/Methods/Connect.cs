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
            try
            {
                using (SqlConnection conn = new SqlConnection(Const.Paths.LocalSQLPath))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand($"SELECT top 1 AccountStockId FROM {Const.Paths.GetDBName()}.[dbo].[AccountStock]", conn))
                    {
                        try
                        {
                            string res = "";
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    res += reader[0].ToString();
                                }
                            }
                            if (res != "") ret = true;
                        }
                        catch (Exception e) { return ret; }
                    }
                    conn.Close();
                }
            }
            catch (Exception e) { }
            return ret;
        }
    }
}
