using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Methods
{
    public static class WriteData
    {
        public static Models.GetBalanceOut FillBalance(Models.FillBalanceIn data, string SQLPath = Const.Paths.LocalSQLPath)
        {
            Models.GetBalanceOut ret = new Models.GetBalanceOut();
            using (SqlConnection conn = new SqlConnection(SQLPath))
            {
                conn.Open();
                string code = data.key;
                using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.UpdateBalance, conn))
                {
                    cmd.Parameters.Add("@key", System.Data.SqlDbType.VarChar);
                    cmd.Parameters.Add("@add_sum", System.Data.SqlDbType.Decimal);
                    cmd.Parameters["@key"].Value = code;
                    cmd.Parameters["@add_sum"].Value = data.add_sum;
                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ret.balance = (decimal)reader[0];
                                ret.key = data.key;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ret.ErrorMessage = e.Message;
                        return ret;
                    }
                }
                conn.Close();
            }

            try
            {
                Models.FillBalanceIn data2 = IsThereEmailPhone(data, SQLPath);
                if(data.email!=data2.email || data.phone != data2.phone)
                {
                    data.email = (data2.email is null || data2.email == "") ? data.email : data2.email;
                    data.phone = (data2.phone is null || data2.phone=="") ? data.phone : data2.phone;
                    UpdateEmail(data, SQLPath);
                }
            }catch(Exception e) { }

            return ret;
        }


        private static Models.FillBalanceIn IsThereEmailPhone(Models.FillBalanceIn data, string SQLPath = Const.Paths.LocalSQLPath)
        {
            Models.FillBalanceIn ret = new Models.FillBalanceIn();
            using (SqlConnection conn = new SqlConnection(SQLPath))
            {
                conn.Open();
                string code = data.key;
                using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.CheckEmail, conn))
                {
                    cmd.Parameters.Add("@key", System.Data.SqlDbType.VarChar);
                    cmd.Parameters["@key"].Value = code;
                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ret.email = reader[0].ToString();
                                ret.phone = reader[1].ToString();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ret.ErrorMessage = e.Message;
                        return ret;
                    }
                }
                conn.Close();
            }
            return ret;
        }


        private static Models.FillBalanceIn UpdateEmail(Models.FillBalanceIn data, string SQLPath = Const.Paths.LocalSQLPath)
        {
            Models.FillBalanceIn ret = new Models.FillBalanceIn();
            using (SqlConnection conn = new SqlConnection(SQLPath))
            {
                conn.Open();
                string code = data.key;
                using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.UpdateEmail, conn))
                {
                    cmd.Parameters.Add("@key", System.Data.SqlDbType.VarChar);
                    cmd.Parameters.Add("@email", System.Data.SqlDbType.VarChar);
                    cmd.Parameters.Add("@phone", System.Data.SqlDbType.VarChar);
                    cmd.Parameters["@key"].Value = code;
                    cmd.Parameters["@email"].Value = data.email;
                    cmd.Parameters["@phone"].Value = data.phone;
                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ret.email = reader[0].ToString();
                                ret.phone = reader[1].ToString();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ret.ErrorMessage = e.Message;
                        return ret;
                    }
                }
                conn.Close();
            }
            return ret;
        }
    }
}
