using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistanceLoader.Core.Harmony.Distance
{
    public class SharpObjectPatch
    {
        public static bool WasSharpEdgeHit(ref bool __result)
        { 
            Util.Logger.Instance.Log($"[SharpObjectPatch-WasSharpEdgeHit] Attempting to return false");

            // Nothing is sharp in our game ;)
            __result = false;
            return true;
        }
    }
}
