using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistanceLoader.Core.Harmony.Distance
{
    public class WingCorruptionZonePatch
    {
        public static bool SetCorruptionEnabled(PlayerDataBase playerData, bool corruptionEnabled, ref  WingCorruptionZone __instance)
        {
            if (Cheats.CheatHandler.Instance.Cheats.AlwaysEnableWings && corruptionEnabled)
            {
                Util.Logger.Instance.Log("[WingCorruptionZonePatch-SetCorruptionEnabled] Stopped a Wing Corruption Zone from disabling flight");
                return false;
            }

            CarLogic carLogic = playerData.CarLogic_;
            WingsGadget wings = carLogic.Wings_;
            if (wings != null)
                wings.SetAbilityEnabled(!corruptionEnabled, __instance.showAbilityAlert);
            carLogic.GetComponent<CutPlaneShaderController>().CorruptionEffectActive_ = corruptionEnabled;

            return false;
        }
    }
}
