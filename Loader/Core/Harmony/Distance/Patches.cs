using System;
using DistanceLoader.Core.Harmony.GenericPatches;
using HarmonyLib;
using NzbDrone.HotPatch.Harmony;

namespace DistanceLoader.Core.Harmony.Distance
{
    class Patches
    {
        public bool ApplyPatches(PatchWrapper patchWrapper)
        {
            try
            {

                Util.Logger.Instance.Log($"[ApplyPatches] Attempting to create new patch for CheatMenu");

                patchWrapper.NewPrefixPatch(
                    AccessTools.Method(typeof(CheatMenu),"OnPanelPop"),
                    AccessTools.Method(typeof(NOP), "NoOperation"));

                patchWrapper.NewPostfixPatch(
                    AccessTools.Method(typeof(CheatMenu), "Awake"),
                    AccessTools.Method(typeof(CheatMenuPatch), "AwakePost"));

                patchWrapper.NewPrefixPatch(
                    AccessTools.Method(typeof(CheatMenu), "CreateMenu"),
                    AccessTools.Method(typeof(CheatMenuPatch), "CheatMenuPre"));

                patchWrapper.NewPrefixPatch(
                    AccessTools.Method(typeof(CheatCodeLogic), "Update"),
                    AccessTools.Method(typeof(CheatCodeLogicPatch), "Update"));

                patchWrapper.NewPrefixPatch(
                    AccessTools.Method(typeof(CheatCodeLogic), "UnlockAllLevels"),
                    AccessTools.Method(typeof(CheatCodeLogicPatch), "UnlockAllLevels"));

                patchWrapper.NewPrefixPatch(
                    AccessTools.Method(typeof(AkGameObj), "Update"),
                    AccessTools.Method(typeof(AkGameObjectPatch), "Update"));

                patchWrapper.NewPrefixPatch(
                    AccessTools.Method(typeof(SharpObject), "WasSharpEdgeHit"),
                    AccessTools.Method(typeof(SharpObjectPatch), "WasSharpEdgeHit"));

                patchWrapper.NewPostfixPatch(
                    AccessTools.Method(typeof(GameManager), "QuitGame"),
                    AccessTools.Method(typeof(GameManagerPatch), "QuitGame"));

                Util.Logger.Instance.Log($"[ApplyPatches] Applying them via Harmony now");
                return patchWrapper.ApplyPatches();
            }
            catch (Exception ex)
            {
                Util.Logger.Instance.Log($"[ApplyPatches] {ex.Message}\r\n\r\n{ex.StackTrace}");
                return false;
            }
        }
    }
}