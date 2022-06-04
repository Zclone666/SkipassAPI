using SkipassAPI.Methods;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Cache
{
    /// <summary>
    /// Renew for cache
    /// </summary>
    public static class Renew
    {
        private static async void _All()
        {
            Cache.Static.ServiceCache.Services = (_RenewServices().Result).services;
            Cache.Static.ServiceCache.ServicesWPrice = (_RenewServicesWPrice().Result).services;
            Cache.Static.ServiceCache.Abons = (_RenewAbons().Result).services;
            Cache.Static.ServiceCache.AbonsWPrice = (_RenewAbonsWPrice().Result).services;
            if (Cache.Init.FirstLaunch)
            {
                if (Cache.Static.ServiceCache.ServicesWPrice.Count == 0)
                {
                    do
                    {

                    } while (Cache.Static.ServiceCache.ServicesWPrice.Count == 0);
                }
            }
                Cache.Static.NewServResp.ListServWPriceResp = Cache.Methods.Convert.OldToNew(Cache.Static.ServiceCache.ServicesWPrice);
                Cache.Static.NewServResp.ListAbonWPriceResp = Cache.Methods.Convert.OldToNew(Cache.Static.ServiceCache.AbonsWPrice);
        }

        /// <summary>
        /// Renew All Cached data
        /// </summary>
        public static async void All()
        {
            await Task.Run(()=>_All());
        }

        private async static Task<Models.ListServ> _RenewServices()
        {
            #region Renew Service Cache
            try
            {
                string SQLPath = Const.Paths.LocalSQLPath;
                #region Inner Work
                Models.ListServ ret = new Models.ListServ();
                using (SqlConnection conn = new SqlConnection(SQLPath))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.GetAllServices, conn))
                    {
                        try
                        {
                            #region MicrosoftSQL transaction creating
                            SqlTransaction trans;
                            trans = conn.BeginTransaction(System.Data.IsolationLevel.Serializable,"Reading From dbo.Category");
                            cmd.Connection = conn;
                            cmd.Transaction = trans;
                            #endregion
                            using (SqlDataReader reader = cmd.ExecuteReaderAsync().Result)
                            {
                                try
                                {
                               //     await trans.CommitAsync();
                                    #region Reading from MicrosoftSQL
                                    while (reader.Read())
                                    {
                                        Models.Services r = new Models.Services();
                                        r.categoryID = int.TryParse(reader[0].ToString(), out int CatId) ? CatId : 0;
                                        r.stockType = int.TryParse(reader[1].ToString(), out int StockType) ? StockType : 0;
                                        r.name = reader[2].ToString();
                                        ret.services.Add(r);
                                    }
                                   // trans.Dispose();
                                    #endregion
                                }
                                #region SQLReading Exception
                                catch (Exception SQLReadEx)
                                {
                                    ret.errors.message = SQLReadEx.Message;
                                    ret.errors.code = 500;
                                    try
                                    {
                               //         trans.Rollback();
                               //         trans.Dispose();
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
                            return ret;
                        }
                        #region Arguments Exception
                        catch (Exception e)
                        {
                            Models.ListServ r = new Models.ListServ() { errors = new Models.Error() { code = 400, message = e.Message } };
                            return ret;
                        }
                        #endregion
                    }
                    await conn.CloseAsync();
                }
                #endregion
            }
            #endregion
            #region Exceptiom
            catch (Exception e)
            {
                return new Models.ListServ() { errors = new Models.Error() { code = 400, message = e.Message } };
            }
            #endregion
        }

        private async static Task<Models.ListServWPrice> _RenewServicesWPrice()
        {
            #region Renew Service Cache
            try
            {
                string SQLPath = Const.Paths.LocalSQLPath;
                #region Inner Work
                Models.ListServWPrice ret = new Models.ListServWPrice();
                using (SqlConnection conn = new SqlConnection(SQLPath))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.GetAllServicesWPrice, conn))
                    {
                        try
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                try
                                {
                                 //   trans.Commit();
                                    #region Reading from MicrosoftSQL
                                    while (reader.Read())
                                    {
                                        Models.ServicesWPrice r = new Models.ServicesWPrice();
                                        r.categoryID = int.TryParse(reader[0].ToString(), out int CatId) ? CatId : 0;
                                        r.stockType = int.TryParse(reader[1].ToString(), out int StockType) ? StockType : 0;
                                        r.name = reader[2].ToString();
                                        r.price = double.TryParse(reader[3].ToString(), out double Price) ? Price : 0;
                                        r.dayT = reader[4].ToString();
                                        r.dayTypeId= int.TryParse(reader[5].ToString(), out int DayTypeId) ? DayTypeId : 0;
                                        ret.services.Add(r);
                                    }
                                    #endregion
                                }
                                #region SQLReading Exception
                                catch (Exception SQLReadEx)
                                {
                                    ret.errors.message = SQLReadEx.Message;
                                    ret.errors.code = 500;
                                }
                                #endregion
                            }
                          //  trans.Dispose();
                            return ret;
                        }
                        #region Arguments Exception
                        catch (Exception e)
                        {
                            Models.ListServWPrice r = new Models.ListServWPrice() { errors = new Models.Error() { code = 400, message = e.Message } };
                            return ret;
                        }
                        #endregion
                    }
                }
                #endregion

            }
            #endregion
            #region Exceptiom
            catch (Exception e)
            {
                return new Models.ListServWPrice() { errors = new Models.Error() { code = 400, message = e.Message } };
            }
            #endregion
        }

        private async static Task<Models.ListServ> _RenewAbons()
        {
            #region Renew Service Cache
            try
            {
                string SQLPath = Const.Paths.LocalSQLPath;
                #region Inner Work
                Models.ListServ ret = new Models.ListServ();
                using (SqlConnection conn = new SqlConnection(SQLPath))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.GetAbon, conn))
                    {
                        try
                        {
                            #region MicrosoftSQL transaction creating
                            SqlTransaction trans;
                            trans = conn.BeginTransaction(System.Data.IsolationLevel.Serializable, "Reading From dbo.Category");
                            cmd.Connection = conn;
                            cmd.Transaction = trans;
                            #endregion
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                try
                                {
                            //        trans.Commit();
                                    #region Reading from MicrosoftSQL
                                    while (reader.Read())
                                    {
                                        Models.Services r = new Models.Services();
                                        r.categoryID = int.TryParse(reader[0].ToString(), out int CatId) ? CatId : 0;
                                        r.stockType = int.TryParse(reader[1].ToString(), out int StockType) ? StockType : 0;
                                        r.name = reader[2].ToString();
                                        ret.services.Add(r);
                                    }
                                    #endregion
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
                        //    trans.Dispose();
                            return ret;
                        }
                        #region Arguments Exception
                        catch (Exception e)
                        {
                            Models.ListServ r = new Models.ListServ() { errors = new Models.Error() { code = 400, message = e.Message } };
                            return ret;
                        }
                        #endregion
                    }
                }
                #endregion
            }
            #endregion
            #region Exceptiom
            catch (Exception e)
            {
                return new Models.ListServ() { errors = new Models.Error() { code = 400, message = e.Message } };
            }
            #endregion
        }

        private async static Task<Models.ListServWPrice> _RenewAbonsWPrice()
        {
            #region Renew Service Cache
            try
            {
                string SQLPath = Const.Paths.LocalSQLPath;
                #region Inner Work
                Models.ListServWPrice ret = new Models.ListServWPrice();
                using (SqlConnection conn = new SqlConnection(SQLPath))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(Const.SQLCommands.GetAllAbonWPrice, conn))
                    {
                        try
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                try
                                {
                       //             trans.Commit();
                                    #region Reading from MicrosoftSQL
                                    while (reader.Read())
                                    {
                                        Models.ServicesWPrice r = new Models.ServicesWPrice();
                                        r.categoryID = int.TryParse(reader[0].ToString(), out int CatId) ? CatId : 0;
                                        r.stockType = int.TryParse(reader[1].ToString(), out int StockType) ? StockType : 0;
                                        r.name = reader[2].ToString();
                                        r.price = double.TryParse(reader[3].ToString(), out double Price) ? Price : 0;
                                        r.dayT = reader[4].ToString();
                                        r.dayTypeId = int.TryParse(reader[5].ToString(), out int DayTypeId) ? DayTypeId : 0;
                                        ret.services.Add(r);
                                    }
                                    #endregion
                                }
                                #region SQLReading Exception
                                catch (Exception SQLReadEx)
                                {
                                    ret.errors.message = SQLReadEx.Message;
                                    ret.errors.code = 500;
                                }
                                #endregion
                            }
                            return ret;
                        }
                        #region Arguments Exception
                        catch (Exception e)
                        {
                            Models.ListServWPrice r = new Models.ListServWPrice() { errors = new Models.Error() { code = 400, message = e.Message } };
                            return ret;
                        }
                        #endregion
                    }
                }
                #endregion
            }
            #endregion
            #region Exceptiom
            catch (Exception e)
            {
                return new Models.ListServWPrice() { errors = new Models.Error() { code = 400, message = e.Message } };
            }
            #endregion
        }


        /// <summary>
        /// Async Service Cache renew
        /// </summary>
        public static async void RenewServices()
        {
            Cache.Static.ServiceCache.Services = (await _RenewServices()).services;
        }

        /// <summary>
        /// Async Services With Price Cache renew
        /// </summary>
        public static async void RenewServicesWPrice()
        {
            Cache.Static.ServiceCache.ServicesWPrice = (await _RenewServicesWPrice()).services;
        }

        /// <summary>
        /// Async Abonements Cache renew
        /// </summary>
        public static async void RenewAbons()
        {
            Cache.Static.ServiceCache.Abons = (await _RenewAbons()).services;
        }

        /// <summary>
        /// Async Abonements With Price Cache renew
        /// </summary>
        public static async void RenewAbonsWPrice()
        {
            Cache.Static.ServiceCache.AbonsWPrice = (await _RenewAbonsWPrice()).services;
        }
    }
}
