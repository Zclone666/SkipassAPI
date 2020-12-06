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
            List<Models.GetBalanceOut> ret = new List<Models.GetBalanceOut>();
            ret = Methods.ReadData.CheckKey(data);
            JsonResult res = new JsonResult(ret);
            return res;
        }

        [HttpPost("/GetBalance")]
        public JsonResult GetBalance(Models.GetBalanceIn data)
        {
            Models.GetBalanceOut ret = new Models.GetBalanceOut();
            ret = Methods.ReadData.GetBalance(data);
            JsonResult res = new JsonResult(ret);
            return res;
        }

        [HttpPost("/Ping")]
        public string Ping()
        {
            return "Pong!";
        }
    }
}
