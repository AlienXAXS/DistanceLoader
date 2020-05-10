using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DistanceLoader.Core.Harmony
{
    public class GUITester : UILabel
    {
        public GUITester()
        {
            Util.Logger.Instance.Log("[GUITester-MonoBehaviour] Constructor");
        }

        private void OnGui()
        {
            try
            {
                Util.Logger.Instance.Log("[GUITester-MonoBehaviour] Rendering");
                GUIStyle guiStyle = new GUIStyle();
                guiStyle.normal.textColor = Color.white;
                string str = "Hello there!";
                UnityEngine.GUI.Label(new Rect(100.0f, 100.0f, (float) Screen.width, (float) Screen.height), str, guiStyle);
            }
            catch (Exception ex)
            {
                Util.Logger.Instance.Log("[GUITester-MonoBehaviour] Rendering Oopsie", ex);
            }
        }
    }
}
