using SkipassAPI.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Cache
{
    /// <summary>
    /// Basic initialisations for caches
    /// </summary>
    public static class Init
    {
        /// <summary>
        /// Flag that indicates whatever is it a first API start or not
        /// </summary>
        public static bool FirstLaunch = true;

        /// <summary>
        /// Main method for cache init
        /// </summary>
        public static void GenerateCacheOnInit()
        {
            if (FirstLaunch)
            {
                Cache.Renew.All();
                FirstLaunch = false;
            }
            else
            {
                Cache.Renew.All();
            }
        }
    }
}
