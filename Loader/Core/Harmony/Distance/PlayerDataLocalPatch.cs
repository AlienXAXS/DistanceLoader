using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DistanceLoader.Core;
using Events.Player;
using UnityEngine;

namespace DistanceLoader.Core.Harmony.Distance
{
    public class PlayerDataLocalPatch
    {

        // Called when a car is loaded (maybe local, maybe all?)
        public static void InitCarVirtual(bool fastRespawn)
        {
            DMLEvents.EventManager.Instance.FireEvent(DMLEvents.EventManager.DMLEvents.OnLocalCarCreated);
        }

        public static void ReinitializeCar()
        {
            DMLEvents.EventManager.Instance.FireEvent(DMLEvents.EventManager.DMLEvents.OnLocalCarReinitialized);
        }
    }
}
