using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityEngine.Networking;

namespace NzbDrone.HotPatch.Harmony
{
    class PatchWrapper
    {
        private readonly HarmonyLib.Harmony _harmonyInstance = new HarmonyLib.Harmony("com.agngaming.distance.patch");
        
        private readonly List<PatchMakeup> _patches = new List<PatchMakeup>();
        private class PatchMakeup
        {
            public readonly MethodInfo originalMethod;
            public readonly HarmonyMethod patchedMethod;
            public bool isPostFix = false;

            public PatchMakeup(MethodInfo arg1, HarmonyMethod arg2, bool isPostFix = false)
            {
                originalMethod = arg1;
                patchedMethod = arg2;
                this.isPostFix = isPostFix;
            }
        }

        public void NewPrefixPatch(MethodInfo originalMethod, MethodInfo patchedMethod)
        {
            if (originalMethod != null)
            {
                if (patchedMethod != null)
                {
                    _patches.Add(new PatchMakeup(originalMethod, new HarmonyMethod(patchedMethod)));
                }
                else
                {
                    DistanceLoader.Util.Logger.Instance.Log($"[NewPrefixPatch] patchedMethod is null!");
                }
            }
            else
            {
                DistanceLoader.Util.Logger.Instance.Log($"[NewPrefixPatch] originalMethod is null!");
            }
        }

        public void NewPostfixPatch(MethodInfo originalMethod, MethodInfo patchedMethod)
        {
            if (originalMethod != null)
            {
                if (patchedMethod != null)
                {
                    _patches.Add(new PatchMakeup(originalMethod, new HarmonyMethod(patchedMethod), true));
                }
                else
                {
                    DistanceLoader.Util.Logger.Instance.Log($"[NewPostfixPatch] patchedMethod is null!");
                }
            }
            else
            {
                DistanceLoader.Util.Logger.Instance.Log($"[NewPostfixPatch] originalMethod is null!");
            }
        }

        /// <summary>
        /// Applies all patches in the order they were sent in.
        /// </summary>
        /// <returns>Returns a bool value if all of the patches were successful or not</returns>
        public bool ApplyPatches()
        {
            try
            {
                DistanceLoader.Util.Logger.Instance.Log($"[ApplyPatches] We have {_patches.Count} patches to process!");

                foreach (var patch in _patches)
                {
                    DistanceLoader.Util.Logger.Instance.Log($"Attempting to patch {patch.originalMethod.Name} in {patch.originalMethod.DeclaringType.FullName}");
                    if (patch.isPostFix)
                        _harmonyInstance.Patch(patch.originalMethod, postfix: patch.patchedMethod);
                    else
                        _harmonyInstance.Patch(patch.originalMethod, patch.patchedMethod, null);
                    DistanceLoader.Util.Logger.Instance.Log($"Patching {patch.originalMethod.Name} successful.");
                }

                return true;
            }
            catch (Exception ex)
            {
                DistanceLoader.Util.Logger.Instance.Log($"ERROR: {ex.Message}\r\n\r\n{ex.InnerException?.Message}");
                return false;
            }
        }
    }
}
