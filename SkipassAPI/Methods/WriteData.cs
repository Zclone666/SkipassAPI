using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Methods
{
    public static class WriteData
    {

        public static Models.AddServiceResp AddServices(Models.AddServiceReq data, string SQLPath = null)
        {
            if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
            Models.AddServiceResp ret = new Models.AddServiceResp();
            if (Methods.CheckAuthkey.CheckAuthKey(data.authkey))
            {
                using (SqlConnection conn = new SqlConnection(SQLPath))
                {
                    conn.Open();
                    string code = data.key;
                    using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.AddAccountStock, conn))
                    {
                        cmd.Parameters.Add("@key", System.Data.SqlDbType.VarChar);
                        cmd.Parameters.Add("@amount", System.Data.SqlDbType.Decimal);
                        cmd.Parameters.Add("@catId", System.Data.SqlDbType.VarChar);
                        cmd.Parameters.Add("@date_start", System.Data.SqlDbType.DateTime);
                        cmd.Parameters.Add("@date_end", System.Data.SqlDbType.DateTime);
                        cmd.Parameters["@key"].Value = code;
                        cmd.Parameters["@amount"].Value = data.amount;
                        cmd.Parameters["@catId"].Value = data.categoryID;
                        cmd.Parameters["@date_start"].Value = Global.UnixTimeToDateTime(data.date_start).ToString();
                        cmd.Parameters["@date_end"].Value = Global.UnixTimeToDateTime(data.date_end).ToString();
                        try
                        {
                            #region MicrosoftSQL transaction creating
                            using (SqlTransaction trans = conn.BeginTransaction(System.Data.IsolationLevel.Serializable, "Writing..."))
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = trans;
                                #endregion
                                //cmd.ExecuteNonQuery();
                                ret.service.accountStockId = (int)cmd.ExecuteScalar();
                                try
                                {
                                    trans.Commit();
                                    ret.isSuccess = true;
                                    data.accountStockId = ret.service.accountStockId;
                                    ret.service = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.AddService>(Newtonsoft.Json.JsonConvert.SerializeObject(data));
                                }
                                #region SQLReading Exception
                                catch (Exception SQLReadEx)
                                {
                                    ret.errors.message = SQLReadEx.Message;
                                    ret.errors.code = 500;
                                    try
                                    {
                                        trans.Rollback();
                                    }
                                    #region Transaction RollBack Exception
                                    catch (Exception RollBEx)
                                    {
                                        ret.errors.message += $"\n{RollBEx.Message}";
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            
                        }
                        catch (Exception e)
                        {
                            ret.errors = new Models.Error() { code = 400, message = e.Message };
                            return ret;
                        }
                    }
                    conn.Close();
                }
                return ret;
            }
            else
            {
                ret.errors = new Models.Error() { code = 401, message = "Unauthorized" };
                return ret;
            }

            Models.FillBalanceIn fb = new Models.FillBalanceIn() { add_sum = data.amount, key = data.key, successed = 1, payment_system = "online" };
            Methods.WriteData.LogHistory(fb);
            //try
            //{
            //    Models.FillBalanceIn data2 = IsThereEmailPhone(data, SQLPath);
            //    if (data.email != data2.email || data.phone != data2.phone)
            //    {
            //        data.email = (data2.email is null || data2.email == "") ? data.email : data2.email;
            //        data.phone = (data2.phone is null || data2.phone == "") ? data.phone : data2.phone;
            //        UpdateEmail(data, SQLPath);
            //    }
            //}
            //catch (Exception e) { }

            //return ret;
        }


        public static Models.CancelServiceResp CancelServices(Models.AddServiceReq data, string SQLPath = null)
        {
            if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
            Models.CancelServiceResp ret = new Models.CancelServiceResp();
            if (data.accountStockId!=0 || Methods.CheckAuthkey.CheckAuthKey(data.authkey))
            {
                if (!Methods.ReadData.CheckAccountStockId(data)) return new Models.CancelServiceResp() { isSuccess = false, errors = new Models.Error() { code = 404, message = "No such AccountStockId or it's cancelled!" } };
                using (SqlConnection conn = new SqlConnection(SQLPath))
                {
                    conn.Open();
                    string code = data.key;
                    string CommOfChoise = (data.accountStockId != 0) ? Const.SQLCommands.CanUSrvAccStockId : Const.SQLCommands.CancelUserSrv;
                    using (SqlCommand cmd = new SqlCommand(CommOfChoise, conn))
                    {
                        if (data.accountStockId == 0)
                        {
                            cmd.Parameters.Add("@key", System.Data.SqlDbType.VarChar);
                            cmd.Parameters.Add("@catId", System.Data.SqlDbType.VarChar);
                            cmd.Parameters.Add("@date_start", System.Data.SqlDbType.DateTime);
                            cmd.Parameters["@key"].Value = code;
                            cmd.Parameters["@catId"].Value = data.categoryID;
                            string dt = Global.UnixTimeToDateTime(data.date_start).ToString();
                            cmd.Parameters["@date_start"].Value = dt;
                        }
                        else
                        {
                            cmd.Parameters.Add("@accStockId", System.Data.SqlDbType.VarChar);
                            cmd.Parameters["@accStockId"].Value = data.accountStockId;
                        }
                        try
                        {
                            #region MicrosoftSQL transaction creating
                            using (SqlTransaction trans = conn.BeginTransaction(System.Data.IsolationLevel.Serializable, "Writing..."))
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = trans;
                            #endregion
                                cmd.ExecuteNonQuery();
                                try
                                {
                                    trans.Commit();
                                    ret.isSuccess = true;
                                }
                                #region SQLReading Exception
                                catch (Exception SQLReadEx)
                                {
                                    ret.errors.message = SQLReadEx.Message;
                                    ret.errors.code = 500;
                                    try
                                    {
                                        trans.Rollback();
                                    }
                                    #region Transaction RollBack Exception
                                    catch (Exception RollBEx)
                                    {
                                        ret.errors.message += $"\n{RollBEx.Message}";
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                        }
                        catch (Exception e)
                        {
                            ret.errors = new Models.Error() { code = 400, message = e.Message };
                            return ret;
                        }
                    }
                    conn.Close();
                }
                return ret;
            }
            else
            {
                ret.errors = new Models.Error() { code = 401, message = "Unauthorized" };
                return ret;
            }

            Models.FillBalanceIn fb = new Models.FillBalanceIn() { add_sum = data.amount, key = data.key, successed = 1, payment_system = "online" };
            Methods.WriteData.LogHistory(fb);
            //try
            //{
            //    Models.FillBalanceIn data2 = IsThereEmailPhone(data, SQLPath);
            //    if (data.email != data2.email || data.phone != data2.phone)
            //    {
            //        data.email = (data2.email is null || data2.email == "") ? data.email : data2.email;
            //        data.phone = (data2.phone is null || data2.phone == "") ? data.phone : data2.phone;
            //        UpdateEmail(data, SQLPath);
            //    }
            //}
            //catch (Exception e) { }

            //return ret;
        }


        public static Models.AddServiceResp CancelSrvByAccStId(Models.AddServiceReq data, string SQLPath = null)
        {
            if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
            Models.AddServiceResp ret = new Models.AddServiceResp();
            if (Methods.CheckAuthkey.CheckAuthKey(data.authkey))
            {
                using (SqlConnection conn = new SqlConnection(SQLPath))
                {
                    conn.Open();
                    string code = data.key;
                    using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.CancelUserSrv, conn))
                    {
                        cmd.Parameters.Add("@key", System.Data.SqlDbType.VarChar);
                        cmd.Parameters.Add("@catId", System.Data.SqlDbType.VarChar);
                        cmd.Parameters.Add("@date_start", System.Data.SqlDbType.DateTime);
                        cmd.Parameters["@key"].Value = code;
                        cmd.Parameters["@catId"].Value = data.categoryID;
                        string dt = Global.UnixTimeToDateTime(data.date_start).ToString();
                        cmd.Parameters["@date_start"].Value = dt;
                        try
                        {
                            #region MicrosoftSQL transaction creating
                            using (SqlTransaction trans = conn.BeginTransaction(System.Data.IsolationLevel.Serializable, "Writing..."))
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = trans;
                                #endregion
                                cmd.ExecuteNonQuery();
                                try
                                {
                                    trans.Commit();
                                    ret.isSuccess = true;
                                }
                                #region SQLReading Exception
                                catch (Exception SQLReadEx)
                                {
                                    ret.errors.message = SQLReadEx.Message;
                                    ret.errors.code = 500;
                                    try
                                    {
                                        trans.Rollback();
                                    }
                                    #region Transaction RollBack Exception
                                    catch (Exception RollBEx)
                                    {
                                        ret.errors.message += $"\n{RollBEx.Message}";
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                        }
                        catch (Exception e)
                        {
                            ret.errors = new Models.Error() { code = 400, message = e.Message };
                            return ret;
                        }
                    }
                    conn.Close();
                }
                return ret;
            }
            else
            {
                ret.errors = new Models.Error() { code = 401, message = "Unauthorized" };
                return ret;
            }

            Models.FillBalanceIn fb = new Models.FillBalanceIn() { add_sum = data.amount, key = data.key, successed = 1, payment_system = "online" };
            Methods.WriteData.LogHistory(fb);
            //try
            //{
            //    Models.FillBalanceIn data2 = IsThereEmailPhone(data, SQLPath);
            //    if (data.email != data2.email || data.phone != data2.phone)
            //    {
            //        data.email = (data2.email is null || data2.email == "") ? data.email : data2.email;
            //        data.phone = (data2.phone is null || data2.phone == "") ? data.phone : data2.phone;
            //        UpdateEmail(data, SQLPath);
            //    }
            //}
            //catch (Exception e) { }

            //return ret;
        }


        public static Models.GetBalanceOut FillBalance(Models.FillBalanceIn data, string SQLPath = null)
        {
            if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
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
                        ret.errors = new Models.Error() { code = 400, message = e.Message };
                        return ret;
                    }
                }
                conn.Close();
            }
            if (!String.IsNullOrEmpty(data.email) && !String.IsNullOrEmpty(data.phone))
            {
                try
                {
                    Models.FillBalanceIn data2 = IsThereEmailPhone(data, SQLPath);
                    if (data.email != data2.email || data.phone != data2.phone)
                    {
                        data.email = (data2.email is null || data2.email == "") ? data.email : data2.email;
                        data.phone = (data2.phone is null || data2.phone == "") ? data.phone : data2.phone;
                        UpdateEmail(data, SQLPath);
                    }
                }
                catch (Exception e) { }
            }
            return ret;
        }


        private static Models.FillBalanceIn IsThereEmailPhone(Models.FillBalanceIn data, string SQLPath = null)
        {
            if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
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
                        ret.errors = new Models.Error() { code = 400, message = e.Message };
                        return ret;
                    }
                }
                conn.Close();
            }
            return ret;
        }


        private static Models.FillBalanceIn UpdateEmail(Models.FillBalanceIn data, string SQLPath = null)
        {
            if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
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
                        ret.errors = new Models.Error() { code = 400, message = e.Message };
                        return ret;
                    }
                }
                conn.Close();
            }
            return ret;
        }

        public static Models.GetBalanceOut LogHistory(Models.FillBalanceIn data, string SQLPath = null)
        {
            if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
            Models.GetBalanceOut ret = new Models.GetBalanceOut();
            using (SqlConnection conn = new SqlConnection(SQLPath))
            {
                conn.Open();
                string code = data.key;
                using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.SaveHistory, conn))
                {
                    cmd.Parameters.Add("@key", System.Data.SqlDbType.VarChar);
                    cmd.Parameters.Add("@add_sum", System.Data.SqlDbType.Decimal);
                    cmd.Parameters.Add("@successed", System.Data.SqlDbType.Bit);
                    cmd.Parameters.Add("@email", System.Data.SqlDbType.VarChar);
                    cmd.Parameters.Add("@phone", System.Data.SqlDbType.VarChar);
                    cmd.Parameters.Add("@payment_id", System.Data.SqlDbType.VarChar);
                    cmd.Parameters.Add("@payment_system", System.Data.SqlDbType.VarChar);
                    cmd.Parameters.Add("@payment_source", System.Data.SqlDbType.VarChar);
                    cmd.Parameters.Add("@comment", System.Data.SqlDbType.VarChar);
                    cmd.Parameters["@key"].Value = code;
                    cmd.Parameters["@add_sum"].Value = data.add_sum;
                    cmd.Parameters["@successed"].Value = data.successed;
                    cmd.Parameters["@email"].Value = data.email;
                    cmd.Parameters["@phone"].Value = data.phone;
                    cmd.Parameters["@payment_id"].Value = (data.payment_id is null)?"":data.payment_id;
                    cmd.Parameters["@payment_source"].Value = (data.payment_source is null)?"":data.payment_source;
                    cmd.Parameters["@payment_system"].Value = (data.payment_system is null)?"":data.payment_system;
                    cmd.Parameters["@comment"].Value = (data.comment is null)?"":data.comment;
                    try
                    {
                        #region MicrosoftSQL transaction creating
                        using (SqlTransaction trans = conn.BeginTransaction(System.Data.IsolationLevel.Serializable, "Writing..."))
                        {
                            cmd.Connection = conn;
                            cmd.Transaction = trans;
                            #endregion
                            cmd.ExecuteNonQuery();
                            try
                            {
                                trans.Commit();
                            }
                            #region SQLReading Exception
                            catch (Exception SQLReadEx)
                            {
                                ret.errors.message = SQLReadEx.Message;
                                ret.errors.code = 500;
                                try
                                {
                                    trans.Rollback();
                                }
                                #region Transaction RollBack Exception
                                catch (Exception RollBEx)
                                {
                                    ret.errors.message += $"\n{RollBEx.Message}";
                                }
                                #endregion
                            }
                            #endregion
                        }
                        //using (SqlDataReader reader = cmd.ExecuteReader())
                        //{
                        //    while (reader.Read())
                        //    {
                        //        ret.balance = (decimal)reader[0];
                        //        ret.key = data.key;
                        //    }
                        //}
                        ret.key = data.key;
                    }
                    catch (Exception e)
                    {
                        ret.errors = new Models.Error() { code = 400, message = e.Message };
                        return ret;
                    }
                }
                conn.Close();
            }

            return ret;
        }

    }
}
