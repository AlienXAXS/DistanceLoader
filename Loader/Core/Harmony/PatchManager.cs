using DistanceLoader.Core.Harmony.Distance;
using NzbDrone.HotPatch.Harmony;

namespace DistanceLoader.Core.Harmony
{
    /// <summary>
    /// Handles Harmony patches depending on the version of Radarr detected
    /// </summary>
    class PatchManager
    {
        private readonly PatchWrapper patchWrapper = new PatchWrapper();

        public bool BeginPatching()
        {
            var patches = new Patches();

            Util.Logger.Instance.Log($"[BeginPatching] Applying patches");
            return patches.ApplyPatches(patchWrapper);
        }
    }
}
