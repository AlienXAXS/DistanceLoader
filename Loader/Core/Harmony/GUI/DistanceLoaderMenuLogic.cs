using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DistanceLoader.Core.Harmony.GUI
{
    public class DistanceLoaderMenuClick : MonoBehaviour
    {

        private void Start()
        {
            Util.Logger.Instance.Log("[DistanceLoaderMenuClick] Start!");
            this.enabled = true;
        }
        
        private void OnClick()
        {
            Util.Logger.Instance.Log("You clicked me!");
        }
    }


    public class DistanceLoaderMenuLogic : MonoBehaviour
    {
        private MenuPanelManager menuPanelManager_;

        public DistanceLoaderMenuLogic()
        {
            Util.Logger.Instance.Log("[DistanceLoaderMenuLogic] Start!");
            this.menuPanelManager_ = G.Sys.MenuPanelManager_;
        }

        public void OnClick()
        {
            Util.Logger.Instance.Log("You clicked me!");
            menuPanelManager_.ShowTimedOk(0, "I'm sorry, this button does not do anything just yet!",
                "Distance Mod Loader Settings", (MessagePanelLogic.OnButtonClicked) null, UIWidget.Pivot.Center);
        }
    }
}
