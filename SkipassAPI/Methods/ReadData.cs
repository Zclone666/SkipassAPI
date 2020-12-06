using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Methods
{
    public static class ReadData
    {
        public static Models.GetBalanceOut GetBalance(Models.GetBalanceIn data, string SQLPath=Const.Paths.LocalSQLPath)
        {
            Models.GetBalanceOut ret = new Models.GetBalanceOut();
            using (SqlConnection conn = new SqlConnection(SQLPath))
            {
                conn.Open();
                string code = data.key;
                using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.GetBalance, conn))
                {
                    cmd.Parameters.Add("@key", System.Data.SqlDbType.VarChar);
                    cmd.Parameters["@key"].Value = code;
                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ret.id = (int)reader[0];
                                ret.key = reader[1].ToString();
                                ret.balance = (decimal)reader[2];
                            }
                        }
                        // if (res != "") ret = true;
                    }
                    catch (Exception e)
                    {
                        Models.GetBalanceOut r = new Models.GetBalanceOut() { ErrorMessage = e.Message };
                        return r;
                    }
                }
                conn.Close();
            }

            return ret;
        }

        public static List<Models.GetBalanceOut> CheckKey(Models.GetBalanceIn data, string SQLPath = Const.Paths.LocalSQLPath)
        {
            List<Models.GetBalanceOut> ret = new List<Models.GetBalanceOut>();
            using (SqlConnection conn = new SqlConnection(SQLPath))
            {
                conn.Open();
                string code = '%'+data.key+'%';
                using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.GetKeyPass, conn))
                {
                    cmd.Parameters.Add("@key", System.Data.SqlDbType.VarChar);
                    cmd.Parameters["@key"].Value = code;
                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.GetBalanceOut r = new Models.GetBalanceOut();
                                r.key = reader[0].ToString();
                                ret.Add(r);
                            }
                        }
                        // if (res != "") ret = true;
                    }
                    catch (Exception e) 
                    {
                        Models.GetBalanceOut r = new Models.GetBalanceOut() { ErrorMessage = e.Message };
                        ret.Add(r);
                        return ret; 
                    }
                }
                conn.Close();
            }


            return ret;
        }

    }
}
