using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Skypass : ControllerBase
    {
        /// <summary>
        /// Страница апи
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            return "Skypass API";
        }

        /// <summary>
        /// Проверка скипасса
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("/CheckKey")]
        public JsonResult CheckKey(Models.GetBalanceIn data)
        {
            bool tst;
            List<Models.GetBalanceOut> ret = new List<Models.GetBalanceOut>();
            try
            {
                ret = Methods.ReadData.CheckKey(data);
                JsonResult res = new JsonResult((ret.Count>0)?new Models.KeyFound() { Founded = true }: new Models.KeyFound() { Founded = false });
                return res;
            }
            catch (Exception e)
            {
                try
                {
                    tst = Methods.Connect.Test();

                    if (tst)
                    {
                        ret = Methods.ReadData.CheckKey(data);
                        JsonResult res = new JsonResult((ret.Count > 0) ? new Models.KeyFound() { Founded = true } : new Models.KeyFound() { Founded = false });
                        return res;
                    }
                    else
                    {
                        ret = Methods.ReadData.CheckKey(data, Const.Paths.SQLPath);
                        JsonResult res = new JsonResult((ret.Count > 0) ? new Models.KeyFound() { Founded = true } : new Models.KeyFound() { Founded = false });
                        return res;
                    }
                }
                catch (Exception e2)
                {
                    ret.Add(new Models.GetBalanceOut() { errors = new Models.Error() { code=500, message = e2.Message } });
                    JsonResult res = new JsonResult(ret);
                    return res;
                }
            }
        }


        /// <summary>
        /// Проверка скипасса с возвращением информации о пользователе (ФИО)
        /// </summary>
        /// <remarks>
        ///  Ключ авторизации = mn5tq8ZTJSmLA6FJ
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("/CheckKeyGetUserInfo")]
        public JsonResult CheckKeyGetUserInfo(Models.GetBalanceIn data)
        {
            bool tst;
            List<Models.User> ret = new List<Models.User>();
            try
            {
                ret = Methods.ReadData.CheckUserRetName(data);
                JsonResult res = new JsonResult(ret);
                return res;
            }
            catch (Exception e)
            {
                try
                {
                    tst = Methods.Connect.Test();

                    if (tst)
                    {
                        ret = Methods.ReadData.CheckUserRetName(data);
                        JsonResult res = new JsonResult((ret.Count > 0) ? new Models.KeyFound() { Founded = true } : new Models.KeyFound() { Founded = false });
                        return res;
                    }
                    else
                    {
                        ret = Methods.ReadData.CheckUserRetName(data, Const.Paths.SQLPath);
                        JsonResult res = new JsonResult((ret.Count > 0) ? new Models.KeyFound() { Founded = true } : new Models.KeyFound() { Founded = false });
                        return res;
                    }
                }
                catch (Exception e2)
                {
                    ret.Add(new Models.User() { errors = new Models.Error() { code = 400, message = e2.Message } });
                    JsonResult res = new JsonResult(ret);
                    return res;
                }
            }
        }

        /// <summary>
        /// Получение списка услуг. Ключ авторизации = mn5tq8ZTJSmLA6FJ
        /// </summary>
        /// <remarks>
        ///  Получение списка услуг. Ключ авторизации = mn5tq8ZTJSmLA6FJ
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("/GetServices")]
        public JsonResult GetServices(Models.GetBalanceIn data)
        {
            JsonResult res;
            try
            {
                res = new JsonResult(Methods.ReadData.GetAllServices(data));
            }
            catch(Exception e)
            {
                res = new JsonResult(e.Message);
            }
            return res;
        }

        /// <summary>
        /// Получение списка услуг с ценами. Ключ авторизации = mn5tq8ZTJSmLA6FJ
        /// </summary>
        /// <remarks>
        ///  Получение списка услуг с ценами. Ключ авторизации = mn5tq8ZTJSmLA6FJ
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("/GetServicesWPrice")]
        public JsonResult GetServicesWPrice(Models.GetBalanceIn data)
        {
            JsonResult res;
            try
            {
                res = new JsonResult(Methods.ReadData.GetAllServicesWPrice(data));
            }
            catch (Exception e)
            {
                res = new JsonResult(e.Message);
            }
            return res;
        }

        /// <summary>
        /// Получение списка АБОНЕМЕНТОВ (без цен). Ключ авторизации = mn5tq8ZTJSmLA6FJ
        /// </summary>
        /// <remarks>
        ///  Получение списка АБОНЕМЕНТОВ (без цен). Ключ авторизации = mn5tq8ZTJSmLA6FJ
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("/GetAbonements")]
        public JsonResult GetAbonements(Models.GetBalanceIn data)
        {
            JsonResult res;
            try
            {
                res = new JsonResult(Methods.ReadData.GetAbonements(data));
            }
            catch (Exception e)
            {
                res = new JsonResult(e.Message);
            }
            return res;
        }

        /// <summary>
        /// Получение списка АБОНЕМЕНТОВ с ценами. Ключ авторизации = mn5tq8ZTJSmLA6FJ
        /// </summary>
        /// <remarks>
        ///  Получение списка АБОНЕМЕНТОВ с ценами. Ключ авторизации = mn5tq8ZTJSmLA6FJ
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("/GetAbonWPrice")]
        public JsonResult GetAbonWPrice(Models.GetBalanceIn data)
        {
            JsonResult res;
            try
            {
                res = new JsonResult(Methods.ReadData.GetAbonWPrice(data));
            }
            catch (Exception e)
            {
                res = new JsonResult(e.Message);
            }
            return res;
        }

        /// <summary>
        /// Получение списка услуг (абонементов)- пользователя по номеру скипасса (key)
        /// </summary>
        /// <remarks>
        ///  Получение списка услуг (абонементов)- пользователя по номеру скипасса (key). Ключ авторизации = mn5tq8ZTJSmLA6FJ
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("/GetUsersAbonements")]
        public JsonResult GetUserAbon(Models.UserServicesReq data)
        {
            JsonResult res;
            try
            {
                res = new JsonResult(Methods.ReadData.GetUserAbon(data));
            }
            catch (Exception e)
            {
                res = new JsonResult(e.Message);
            }
            return res;
        }

        /// <summary>
        /// Получение списка услуг (ВСЕХ)- пользователя по номеру скипасса (key)
        /// </summary>
        /// <remarks>
        ///  Получение списка услуг (абонементов)- пользователя по номеру скипасса (key). Ключ авторизации = mn5tq8ZTJSmLA6FJ
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("/GetUsersServices")]
        public JsonResult GetUserSrv(Models.UserServicesReq data)
        {
            JsonResult res;
            try
            {
                res = new JsonResult(Methods.ReadData.GetUserServices(data));
            }
            catch (Exception e)
            {
                res = new JsonResult(e.Message);
            }
            return res;
        }

        /// <summary>
        /// Получение баланса юзера (зима)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("/GetBalance")]
        public JsonResult GetBalance(Models.GetBalanceIn data)
        {
            bool tst;
            Models.GetBalanceOut ret = new Models.GetBalanceOut();
            JsonResult res;
            try
            {
                ret = Methods.ReadData.GetBalance(data);
                res = new JsonResult(ret);
                return res;
            }
            catch (Exception e)
            {
                try
                {
                    tst = Methods.Connect.Test();
                    
                    if (tst)
                    {
                        ret = Methods.ReadData.GetBalance(data);
                        res = new JsonResult(ret);
                        return res;
                    }
                    else
                    {
                        ret = Methods.ReadData.GetBalance(data, Const.Paths.SQLPath);
                        res = new JsonResult(ret);
                        return res;
                    }
                }
                catch (Exception e2)
                {
                    res = new JsonResult(new Models.GetBalanceOut() { errors = new Models.Error() { code = 500, message = e2.Message } });
                    return res;
                }
                res = new JsonResult(new Models.GetBalanceOut() { errors = new Models.Error() { code = 400, message = e.Message } });
                return res;
            }
        }


        /// <summary>
        /// Добавление денег на депозит (зима)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("/AddSum")]
        public JsonResult AddSum(Models.FillBalanceIn data)
        {
            try
            {
                if (data.successed==1)
                {
                    bool tst;
                    Models.GetBalanceOut ret = new Models.GetBalanceOut();
                    try
                    {
                        ret = Methods.WriteData.FillBalance(data);
                        Methods.WriteData.LogHistory(data);
                        JsonResult res = new JsonResult(ret);
                        return res;
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            tst = Methods.Connect.Test();

                            if (tst)
                            {
                                ret = Methods.WriteData.FillBalance(data);
                                Methods.WriteData.LogHistory(data);
                                JsonResult res = new JsonResult(ret);
                                return res;
                            }
                            else
                            {
                                ret = Methods.WriteData.FillBalance(data, Const.Paths.SQLPath);
                                Methods.WriteData.LogHistory(data, Const.Paths.SQLPath);
                                JsonResult res = new JsonResult(ret);
                                return res;
                            }
                        }
                        catch (Exception e2)
                        {
                            JsonResult res = new JsonResult(new Models.GetBalanceOut() { errors = new Models.Error() { code = 400, message = e2.Message } });
                            return res;
                        }
                    }
                }
                else
                {
                    bool tst;
                    Models.GetBalanceOut ret = new Models.GetBalanceOut();
                    try
                    {
                        ret = Methods.WriteData.LogHistory(data);
                        JsonResult res = new JsonResult(ret);
                        return res;
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            tst = Methods.Connect.Test();

                            if (tst)
                            {
                                ret = Methods.WriteData.LogHistory(data);
                                JsonResult res = new JsonResult(ret);
                                return res;
                            }
                            else
                            {
                                ret = Methods.WriteData.LogHistory(data, Const.Paths.SQLPath);
                                JsonResult res = new JsonResult(ret);
                                return res;
                            }
                        }
                        catch (Exception e2)
                        {
                            JsonResult res = new JsonResult(new Models.GetBalanceOut() { errors = new Models.Error() { code = 400, message = e2.Message } });
                            return res;
                        }
                    }
                }
            }catch(Exception er)
            {
                JsonResult res = new JsonResult(new Models.GetBalanceOut() { errors = new Models.Error() { code = 500, message = er.Message } });
                return res;
            }
        }

        /// <summary>
        /// Добавление услуг на номер скипасса. Ключ авторизации = mn5tq8ZTJSmLA6FJ
        /// </summary>
        /// <remarks>
        /// Получение списка услуг. Ключ авторизации = mn5tq8ZTJSmLA6FJ
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("/AddServiceToUser")]
        public JsonResult AddService(Models.AddService data)
        {
            try
            {
                JsonResult res = new JsonResult(Methods.WriteData.AddServices(data));
                return res;
            }
            catch(Exception e)
            {
                return new JsonResult(new Models.AddServiceResp() { errors = new Models.Error() { code = 400, message = e.Message }, isSuccess = false });
            }
        }

        /// <summary>
        /// Отмена услуг на скипассе
        /// </summary>
        /// <remarks>
        ///  Отмена услуг на скипассе. Обязательные поля:
        ///    key - номер скипасса
        ///    date_start - дата начала действия услуги
        ///    categoryId - ID услуги
        ///    authkey - mn5tq8ZTJSmLA6FJ
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("/CancelUserService")]
        public JsonResult CancelService(Models.AddService data)
        {
            try
            {
                JsonResult res = new JsonResult(Methods.WriteData.CancelServices(data));
                return res;
            }
            catch (Exception e)
            {
                return new JsonResult(new Models.AddServiceResp() { errors = new Models.Error() { code = 400, message = e.Message }, isSuccess = false });
            }
        }

        /// <summary>
        /// Понг
        /// </summary>
        /// <returns></returns>
        [HttpPost("/Ping")]
        public string Ping()
        {
            return "Pong!";
        }
    }
}
