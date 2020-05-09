using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DistanceLoader.Core.Harmony.Distance
{
    class AkGameObjectPatch
    {
        public static bool Update(AkGameObj __instance)
        {
            var pos = __instance.GetPosition();
            //Util.Logger.Instance.Log($"[AkGameObjectPatch-Update] Car Position: x:{pos.x} y:{pos.y} z{pos.z}");

            return true;
        }
    }
}
