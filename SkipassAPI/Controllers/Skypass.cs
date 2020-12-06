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
        [HttpGet]
        public string Get()
        {
            return "Skypass API";
        }

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
                    ret.Add(new Models.GetBalanceOut() { ErrorMessage = e2.Message });
                    JsonResult res = new JsonResult(ret);
                    return res;
                }
            }
        }

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
                    res = new JsonResult(new Models.GetBalanceOut() { ErrorMessage = e2.Message });
                    return res;
                }
                res = new JsonResult(new Models.GetBalanceOut() { ErrorMessage = e.Message });
                return res;
            }
        }

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
                            JsonResult res = new JsonResult(new Models.GetBalanceOut() { ErrorMessage = e2.Message });
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
                            JsonResult res = new JsonResult(new Models.GetBalanceOut() { ErrorMessage = e2.Message });
                            return res;
                        }
                    }
                }
            }catch(Exception er)
            {
                JsonResult res = new JsonResult(new Models.GetBalanceOut() { ErrorMessage = er.Message });
                return res;
            }
        }


        [HttpPost("/Ping")]
        public string Ping()
        {
            return "Pong!";
        }
    }
}
