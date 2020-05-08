using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistanceLoader.Util
{
    public class Configuration
    {
        public static Configuration Instance = _instance ?? (_instance = new Configuration());
        private static readonly Configuration _instance;

        public bool IsDebug;

    }
}
