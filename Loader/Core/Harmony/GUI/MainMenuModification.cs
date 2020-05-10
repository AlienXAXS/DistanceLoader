using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DistanceLoader.Core.Harmony.GUI
{
    public class MainMenuModification : SuperMenu
    {

        public override string Name_ => "ModdedMenu";
        public override string MenuTitleName_ => "ModdedMenu";

        public override void OnPanelPop()
        {
            // Called when the menu is closed
        }

        public override bool DisplayInMenu(bool isPauseMenu)
        {
            return true;
        }

        public override void InitializeVirtual()
        {
            // Called when the menu is started
            Util.Logger.Instance.Log($"[MainMenuModification-InitializeVirtual] Called");
        }
    }
}
