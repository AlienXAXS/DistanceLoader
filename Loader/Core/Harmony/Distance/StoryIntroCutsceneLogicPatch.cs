using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistanceLoader.Core.Harmony.Distance
{
    public class StoryIntroCutsceneLogicPatch
    {
        // Called when the car gets created
        public static void OnEventPostLoad()
        {
            Util.Logger.Instance.Log("[StoryIntroCutsceneLogicPatch-OnEventPostLoad] Car Created!");
        }
    }
}
