using SkipassAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Cache.Static
{
    /// <summary>
    /// Static object serving as a cache for services
    /// </summary>
    public static class ServiceCache
    {
        /// <summary>
        /// Static List of Services with Price
        /// </summary>
        public static List<ServicesWPrice> ServicesWPrice { get; set; } = new List<ServicesWPrice>();

        /// <summary>
        /// Static List of Services WITHOUT Price
        /// </summary>
        public static List<Services> Services { get; set; } = new List<Services>();

        /// <summary>
        /// Static List of only Abonements with Price
        /// </summary>
        public static List<ServicesWPrice> AbonsWPrice { get; set; } = new List<ServicesWPrice>();

        /// <summary>
        /// Static List of Abonements WITHOUT Price
        /// </summary>
        public static List<Services> Abons { get; set; } = new List<Services>();
    }
}
