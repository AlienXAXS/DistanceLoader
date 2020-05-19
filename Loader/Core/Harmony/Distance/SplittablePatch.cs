using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DistanceLoader.Core.Cheats;

namespace DistanceLoader.Core.Harmony.Distance
{
    public class SplittablePatch
    {

        // Cheat
        // Stops lasers from being able to slice your car
        public static bool TestAgainstLaser()
        {
            return !CheatHandler.Instance.Cheats.LasersNoDamage;
        }

    }
}
