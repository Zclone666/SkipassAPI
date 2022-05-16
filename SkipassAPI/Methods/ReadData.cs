using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Methods
{
    public static class ReadData
    {
        /// <summary>
        /// Получение баланса
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
                        // if (res != "") ret = true;
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
        /// Получение всех услуг в базе
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
                    using (SqlConnection conn = new SqlConnection(SQLPath))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.GetAllServices, conn))
                        {
                            try
                            {
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Models.Services r = new Models.Services();
                                        r.categoryID = int.TryParse(reader[0].ToString(), out int CatId) ? CatId : 0;
                                        r.stockType = int.TryParse(reader[1].ToString(), out int StockType) ? StockType : 0;
                                        r.name = reader[2].ToString();
                                        ret.services.Add(r);
                                    }
                                }
                                return ret;
                                // if (res != "") ret = true;
                            }
                            catch (Exception e)
                            {
                                Models.ListServ r = new Models.ListServ() { errors = new Models.Error() { code = 400, message = e.Message } };
                                return ret;
                            }
                        }
                    }
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
                    using (SqlConnection conn = new SqlConnection(SQLPath))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.GetAbon, conn))
                        {
                            try
                            {
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Models.Services r = new Models.Services();
                                        r.categoryID = int.TryParse(reader[0].ToString(), out int CatId) ? CatId : 0;
                                        r.stockType = int.TryParse(reader[1].ToString(), out int StockType) ? StockType : 0;
                                        r.name = reader[2].ToString();
                                        ret.services.Add(r);
                                    }
                                }
                                return ret;
                                // if (res != "") ret = true;
                            }
                            catch (Exception e)
                            {
                                Models.ListServ r = new Models.ListServ() { errors = new Models.Error() { code = 400, message = e.Message } };
                                return r;
                            }
                        }
                    }
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
        /// Получение всех услуг с ценами
        /// </summary>
        /// <param name="data"></param>
        /// <param name="SQLPath"></param>
        /// <returns></returns>
        public static Models.ListServWPrice GetAllServicesWPrice(Models.GetBalanceIn data, string SQLPath = null)
        {
            try
            {
                if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
                Models.ListServWPrice ret = new Models.ListServWPrice();
                if (Methods.CheckAuthkey.CheckAuthKey(data.authkey))
                {
                    using (SqlConnection conn = new SqlConnection(SQLPath))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.GetAllServicesWPrice, conn))
                        {
                            try
                            {
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Models.ServicesWPrice r = new Models.ServicesWPrice();
                                        r.categoryID = int.TryParse(reader[0].ToString(), out int CatId) ? CatId : 0;
                                        r.stockType = int.TryParse(reader[1].ToString(), out int StockType) ? StockType : 0;
                                        r.name = reader[2].ToString();
                                        r.price= double.TryParse(reader[3].ToString(), out double Price) ? Price : 0;
                                        r.dayT = reader[4].ToString();
                                        ret.services.Add(r);
                                    }
                                }
                                return ret;
                                // if (res != "") ret = true;
                            }
                            catch (Exception e)
                            {
                                Models.ListServWPrice r = new Models.ListServWPrice() { errors = new Models.Error() { code = 400, message = e.Message } };
                                return r;
                            }
                        }
                    }
                }
                else
                {
                    ret=new Models.ListServWPrice() { errors = new Models.Error() { code = 401, message = "Unauthorized" } };
                    return ret;
                }
            }
            catch (Exception e)
            {
                return new Models.ListServWPrice(){ errors = new Models.Error() { code = 400, message = e.Message } };
            }
        }


        /// <summary>
        /// Получение всех абонементов с ценами
        /// </summary>
        /// <param name="data"></param>
        /// <param name="SQLPath"></param>
        /// <returns></returns>
        public static Models.ListServWPrice GetAbonWPrice(Models.GetBalanceIn data, string SQLPath = null)
        {
            try
            {
                if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
                Models.ListServWPrice ret = new Models.ListServWPrice();
                if (Methods.CheckAuthkey.CheckAuthKey(data.authkey))
                {
                    using (SqlConnection conn = new SqlConnection(SQLPath))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.GetAllAbonWPrice, conn))
                        {
                            try
                            {
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Models.ServicesWPrice r = new Models.ServicesWPrice();
                                        r.categoryID = int.TryParse(reader[0].ToString(), out int CatId) ? CatId : 0;
                                        r.stockType = int.TryParse(reader[1].ToString(), out int StockType) ? StockType : 0;
                                        r.name = reader[2].ToString();
                                        r.price = double.TryParse(reader[3].ToString(), out double Price) ? Price : 0;
                                        r.dayT = reader[4].ToString();
                                        ret.services.Add(r);
                                    }
                                }
                                return ret;
                                // if (res != "") ret = true;
                            }
                            catch (Exception e)
                            {
                                Models.ListServWPrice r = new Models.ListServWPrice() { errors = new Models.Error() { code = 400, message = e.Message } };
                                return r;
                            }
                        }
                    }
                }
                else
                {
                    ret=new Models.ListServWPrice() { errors = new Models.Error() { code = 401, message = "Unauthorized" } };
                    return ret;
                }
            }
            catch (Exception e)
            {
                return new Models.ListServWPrice() { errors = new Models.Error() { code = 400, message = e.Message } };
            }
        }


        /// <summary>
        /// Получение списка абонементов юзера
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
                                // if (res != "") ret = true;
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
        /// Получение услуг юзера (всех, кроме покупки скипасса)
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
                                // if (res != "") ret = true;
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
        /// Проверка ключа
        /// </summary>
        /// <param name="data"></param>
        /// <param name="SQLPath"></param>
        /// <returns></returns>
        public static List<Models.GetBalanceOut> CheckKey(Models.GetBalanceIn data, string SQLPath = null)
        {
            if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
            List<Models.GetBalanceOut> ret = new List<Models.GetBalanceOut>();
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
                                ret.Add(r);
                            }
                        }
                        // if (res != "") ret = true;
                    }
                    catch (Exception e) 
                    {
                        Models.GetBalanceOut r = new Models.GetBalanceOut() { errors = new Models.Error() { code = 400, message = e.Message } };
                        ret.Add(r);
                        return ret; 
                    }
                }
                conn.Close();
            }
            return ret;
        }

        /// <summary>
        /// Проверка СКИПАССА и возвращение инфы о пользователе по нему
        /// </summary>
        /// <param name="data"></param>
        /// <param name="SQLPath"></param>
        /// <returns></returns>
        public static List<Models.User> CheckUserRetName(Models.GetBalanceIn data, string SQLPath = null)
        {
            if (SQLPath is null) SQLPath = Const.Paths.LocalSQLPath;
            List<Models.User> ret = new List<Models.User>();
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
                                r.lastname = reader[1].ToString();
                                r.firstName = reader[2].ToString();
                                r.middlename = reader[3].ToString();
                                ret.Add(r);
                            }
                        }
                        // if (res != "") ret = true;
                    }
                    catch (Exception e)
                    {
                        Models.User r = new Models.User() { errors = new Models.Error() { code = 400, message = e.Message } };
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
