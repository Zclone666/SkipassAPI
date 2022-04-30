using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Methods
{
    public static class CheckAuthkey
    {
        public static bool CheckAuthKey(string authkey)
        {
            if (authkey is null) return false;
            else
            {
                if (authkey == Const.Key.authkey) return true;
                else return false;
            }
        }
    }
}
