﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Methods
{
    public static class ReadData
    {
        /// <summary>
        /// Get balance
        /// </summary>
        /// <param name="data"></param>
        /// <param name="SQLPath"></param>
        /// <returns></returns>
        public static Models.GetBalanceOut GetBalance(Models.GetBalanceIn data, string SQLPath=null)
        {
            if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
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
                    }
                    catch (Exception e)
                    {
                        Models.GetBalanceOut r = new Models.GetBalanceOut() { errors = new Models.Error() { code = 400, message = e.Message } };
                        return r;
                    }
                }
                conn.Close();
            }

            return ret;
        }

        /// <summary>
    /// Caching services from MS SQL DB
        /// </summary>
        /// <param name="data"></param>
        /// <param name="SQLPath"></param>
        /// <returns></returns>
        public static Models.ListServ GetAllServices(Models.GetBalanceIn data, string SQLPath = null)
        {
            try
            {
                if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
                Models.ListServ ret = new Models.ListServ();
                if (Methods.CheckAuthkey.CheckAuthKey(data.authkey))
                {
                    ret.services = Cache.Static.ServiceCache.Services;
                    return ret;
                }
                else
                {
                    ret=new Models.ListServ() { errors = new Models.Error() { code = 401, message = "Unauthorized" } };
                    return ret;
                }
            }
            catch(Exception e)
            {
                return new Models.ListServ() { errors = new Models.Error() { code = 400, message = e.Message } };
            }
        }

        /// <summary>
        /// Получение всех абонементов в базе
        /// </summary>
        /// <param name="data"></param>
        /// <param name="SQLPath"></param>
        /// <returns></returns>
        public static Models.ListServ GetAbonements(Models.GetBalanceIn data, string SQLPath = null)
        {
            try
            {
                if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
                Models.ListServ ret = new Models.ListServ();
                if (Methods.CheckAuthkey.CheckAuthKey(data.authkey))
                {
                    ret.services = Cache.Static.ServiceCache.Abons;
                    return ret;
                }
                else
                {
                    ret=new Models.ListServ() { errors = new Models.Error() { code = 401, message = "Unauthorized" } };
                    return ret;
                }
            }
            catch (Exception e)
            {
                return new Models.ListServ() { errors = new Models.Error() { code = 400, message = e.Message } };
            }
        }


        /// <summary>
        /// Caching services with prices
        /// </summary>
        /// <param name="data"></param>
        /// <param name="SQLPath"></param>
        /// <returns></returns>
        public static Models.ListServWPriceResp GetAllServicesWPrice(Models.GetBalanceIn data, string SQLPath = null)
        {
            try
            {
                if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
                Models.ListServWPriceResp ret = new Models.ListServWPriceResp();
                if (Methods.CheckAuthkey.CheckAuthKey(data.authkey))
                {
                    return Cache.Static.NewServResp.ListServWPriceResp;
                    var tmpsrv = new List<Models.BaseServResp>();
                    
                    foreach(var i in Cache.Static.ServiceCache.ServicesWPrice.Select(x=>x.categoryID).Distinct())
                    {
                        var tmpprices = new List<Models.PriceResp>();
                        foreach(var p in Cache.Static.ServiceCache.ServicesWPrice.Where(x => x.categoryID == i))
                        {
                            tmpprices.Add(new Models.PriceResp() { dayT = p.dayT, dayTypeId = p.dayTypeId, price = p.price });
                        }
                        tmpsrv.Add(new Models.BaseServResp() { categoryID = i, name = Cache.Static.ServiceCache.ServicesWPrice.Where(x => x.categoryID == i).Select(x => x.name).FirstOrDefault(), stockType = Cache.Static.ServiceCache.ServicesWPrice.Where(x => x.categoryID == i).Select(x => x.stockType).FirstOrDefault(), price=tmpprices });
                    }
                    ret.services = tmpsrv;
                    return ret;
                }
                else
                {
                    ret=new Models.ListServWPriceResp() { errors = new Models.Error() { code = 401, message = "Unauthorized" } };
                    return ret;
                }
            }
            catch (Exception e)
            {
                return new Models.ListServWPriceResp(){ errors = new Models.Error() { code = 400, message = e.Message } };
            }
        }


        /// <summary>
        /// Caching subscriptions with prices
        /// </summary>
        /// <param name="data"></param>
        /// <param name="SQLPath"></param>
        /// <returns></returns>
        public static Models.ListServWPriceResp GetAbonWPrice(Models.GetBalanceIn data, string SQLPath = null)
        {
            try
            {
                if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
                Models.ListServWPriceResp ret = new Models.ListServWPriceResp();
                if (Methods.CheckAuthkey.CheckAuthKey(data.authkey))
                {
                    return Cache.Static.NewServResp.ListAbonWPriceResp;
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
                    return ret;
                }
                else
                {
                    ret=new Models.ListServWPriceResp() { errors = new Models.Error() { code = 401, message = "Unauthorized" } };
                    return ret;
                }
            }
            catch (Exception e)
            {
                return new Models.ListServWPriceResp() { errors = new Models.Error() { code = 400, message = e.Message } };
            }
        }


        /// <summary>
        /// Get user's subscribtions
        /// </summary>
        /// <param name="data"></param>
        /// <param name="SQLPath"></param>
        /// <returns></returns>
        public static Models.UserServicesResp GetUserAbon(Models.UserServicesReq data, string SQLPath = null)
        {
            try
            {
                if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
                Models.UserServicesResp ret = new Models.UserServicesResp();
                if (Methods.CheckAuthkey.CheckAuthKey(data.authkey))
                {
                    using (SqlConnection conn = new SqlConnection(SQLPath))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.GetUserAbon, conn))
                        {
                            cmd.Parameters.Add("@key", System.Data.SqlDbType.VarChar);
                            cmd.Parameters["@key"].Value = data.key;
                            try
                            {
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Models.UserServices r = new Models.UserServices();
                                        r.servName = reader[0].ToString();
                                        r.isActive = bool.TryParse(reader[1].ToString(), out bool IsActive) ? IsActive : false;
                                        r.amount = double.TryParse(reader[2].ToString(), out double Amount) ? Amount : 0;
                                        r.start = reader[3].ToString();
                                        r.end = reader[4].ToString();
                                        ret.services.Add(r);
                                    }
                                }
                                return ret;
                            }
                            catch (Exception e)
                            {
                                Models.UserServicesResp r = new Models.UserServicesResp() { errors = new Models.Error() { code = 400, message = e.Message } };
                                return r;
                            }
                        }
                    }
                }
                else
                {
                    ret = new Models.UserServicesResp() { errors = new Models.Error() { code = 401, message = "Unauthorized" } };
                    return ret;
                }
            }
            catch (Exception e)
            {
                return new Models.UserServicesResp() { errors = new Models.Error() { code = 400, message = e.Message } };
            }
        }


        /// <summary>
        /// Get all services attached to the user (except for Skipass card aquirement)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="SQLPath"></param>
        /// <returns></returns>
        public static Models.UserServicesResp GetUserServices(Models.UserServicesReq data, string SQLPath = null)
        {
            try
            {
                if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
                Models.UserServicesResp ret = new Models.UserServicesResp();
                if (Methods.CheckAuthkey.CheckAuthKey(data.authkey))
                {
                    using (SqlConnection conn = new SqlConnection(SQLPath))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.GetUserServices, conn))
                        {
                            cmd.Parameters.Add("@key", System.Data.SqlDbType.VarChar);
                            cmd.Parameters["@key"].Value = data.key;
                            try
                            {
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Models.UserServices r = new Models.UserServices();
                                        r.servName = reader[0].ToString();
                                        r.isActive = bool.TryParse(reader[1].ToString(), out bool IsActive) ? IsActive : false;
                                        r.amount = double.TryParse(reader[2].ToString(), out double Amount) ? Amount : 0;
                                        r.start = reader[3].ToString();
                                        r.end = reader[4].ToString();
                                        ret.services.Add(r);
                                    }
                                }
                                return ret;
                            }
                            catch (Exception e)
                            {
                                Models.UserServicesResp r = new Models.UserServicesResp() { errors = new Models.Error() { code = 400, message = e.Message } };
                                return r;
                            }
                        }
                    }
                }
                else
                {
                    ret = new Models.UserServicesResp() { errors = new Models.Error() { code = 401, message = "Unauthorized" } };
                    return ret;
                }
            }
            catch (Exception e)
            {
                return new Models.UserServicesResp() { errors = new Models.Error() { code = 400, message = e.Message } };
            }
        }


        /// <summary>
        /// Check skipass id
        /// </summary>
        /// <param name="data"></param>
        /// <param name="SQLPath"></param>
        /// <returns></returns>
        public static Models.GetBalanceOut CheckKey(Models.GetBalanceIn data, string SQLPath = null)
        {
            if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
            Models.GetBalanceOut ret = new Models.GetBalanceOut();
            if (Methods.CheckAuthkey.CheckAuthKey(data.authkey))
            {
                using (SqlConnection conn = new SqlConnection(SQLPath))
                {
                    conn.Open();
                    string code = data.key;
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
                                    ret=r;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Models.GetBalanceOut r = new Models.GetBalanceOut() { errors = new Models.Error() { code = 400, message = e.Message } };
                            ret=r;
                            return ret;
                        }
                    }
                    conn.Close();
                }
                return ret;
            }
            else
            {
                ret = new Models.GetBalanceOut() { errors = new Models.Error() { code = 401, message = "Unauthorized" } };
                return ret;
            }
        }

        /// <summary>
        /// Check skipass id and return user info
        /// </summary>
        /// <param name="data"></param>
        /// <param name="SQLPath"></param>
        /// <returns></returns>
        public static Models.User CheckUserRetName(Models.GetBalanceIn data, string SQLPath = null)
        {
            if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
            Models.User ret = new Models.User();
            if (Methods.CheckAuthkey.CheckAuthKey(data.authkey))
            {
                using (SqlConnection conn = new SqlConnection(SQLPath))
                {
                    conn.Open();
                    string code = data.key;
                    using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.GetKeyPassAndName, conn))
                    {
                        cmd.Parameters.Add("@key", System.Data.SqlDbType.VarChar);
                        cmd.Parameters["@key"].Value = code;
                        try
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Models.User r = new Models.User();
                                    r.userInfo.key = reader[0].ToString();
                                    r.userInfo.lastName = reader[1].ToString();
                                    r.userInfo.firstName = reader[2].ToString();
                                    r.userInfo.middleName = reader[3].ToString();
                                    r.userInfo.email = reader[4].ToString();
                                    r.userInfo.phone = reader[5].ToString();
                                    r.userInfo.isActive = Boolean.TryParse(reader[6].ToString(), out bool IsAct)?IsAct:false;
                                    ret=r;
                                    ret.founded = true;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Models.User r = new Models.User() { errors = new Models.Error() { code = 400, message = e.Message } };
                            ret=r;
                            ret.founded = false;
                            return ret;
                        }
                    }
                    conn.Close();
                }


                return ret;
            }
            else
            {
                ret = new Models.User()
                {
                    errors = new Models.Error() { code = 401, message = "Unauthorized" }
                };
                return ret;
            }
        }

        public static Models.UserInfoList GetCodeByPhoneOrEmail(Models.GetCodeBReq data, string SQLPath = null)
        {
            if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
            Models.UserInfoList ret = new Models.UserInfoList();
            if (Methods.CheckAuthkey.CheckAuthKey(data.authkey))
            {
                using (SqlConnection conn = new SqlConnection(SQLPath))
                {
                    conn.Open();
                    string Email= String.IsNullOrEmpty(data.email)?"":data.email;
                    string Phone = String.IsNullOrEmpty(data.phone) ? "" : data.phone;
                    string CmdtToUse = String.IsNullOrEmpty(data.email) ? Const.SQLCommands.GetCodeByPhone :(String.IsNullOrEmpty(data.phone)? Const.SQLCommands.GetCodeByEmail: Const.SQLCommands.GetCodeByBoth);
                    using (SqlCommand cmd = new SqlCommand(CmdtToUse, conn))
                    {
                        if (String.IsNullOrEmpty(data.email) && !String.IsNullOrEmpty(data.phone))
                        {
                            cmd.Parameters.Add("@phone", System.Data.SqlDbType.VarChar);
                            cmd.Parameters["@phone"].Value = Phone;
                        }else if (String.IsNullOrEmpty(data.email) && String.IsNullOrEmpty(data.phone))
                        {
                            ret = new Models.UserInfoList()
                            {
                                errors = new Models.Error() { code = 400, message = "Specify email or phone!" }
                            };
                            return ret;
                        }
                        else if(!String.IsNullOrEmpty(data.email) && String.IsNullOrEmpty(data.phone))
                        {
                            cmd.Parameters.Add("@email", System.Data.SqlDbType.VarChar);
                            cmd.Parameters["@email"].Value = Email;
                        }
                        else if (!String.IsNullOrEmpty(data.email) && !String.IsNullOrEmpty(data.phone))
                        {
                            cmd.Parameters.Add("@phone", System.Data.SqlDbType.VarChar);
                            cmd.Parameters["@phone"].Value = Phone;
                            cmd.Parameters.Add("@email", System.Data.SqlDbType.VarChar);
                            cmd.Parameters["@email"].Value = Email;
                        }
                        try
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Models.UserInfo r = new Models.UserInfo();
                                    r.key = reader[0].ToString();
                                    r.lastName = reader[1].ToString();
                                    r.firstName = reader[2].ToString();
                                    r.middleName = reader[3].ToString();
                                    r.email = reader[4].ToString();
                                    r.phone = reader[5].ToString();
                                    r.isActive = Boolean.TryParse(reader[6].ToString(), out bool IsAct)?IsAct:false;
                                    ret.userInfo.Add(r);
                                    ret.founded = true;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Models.UserInfoList r = new Models.UserInfoList() { errors = new Models.Error() { code = 400, message = e.Message } };
                            ret = r;
                            ret.founded = false;
                            return ret;
                        }
                    }
                    conn.Close();
                }


                return ret;
            }
            else
            {
                ret = new Models.UserInfoList()
                {
                    errors = new Models.Error() { code = 401, message = "Unauthorized" }
                };
                return ret;
            }
        }

        public static bool CheckAccountStockId(Models.AddServiceReq data)
        {
            using (SqlConnection conn = new SqlConnection(Const.Paths.LocalSQLPath))
            {
                conn.Open();
                int AccountStockId = data.accountStockId;
                if (AccountStockId == 0) return false;
                using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.GetAccountStockId, conn))
                {
                    cmd.Parameters.Add("@accStockId", System.Data.SqlDbType.VarChar);
                    cmd.Parameters["@accStockId"].Value = AccountStockId;
                    try
                    {

                        AccountStockId = (int)cmd.ExecuteScalar();
                        if (AccountStockId == data.accountStockId) return true;
                        else return false;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
                conn.Close();
            }
        }

    }
}
