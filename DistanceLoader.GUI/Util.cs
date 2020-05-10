using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DistanceLoader.GUI
{
    public class Util
    {
        public static void SetTextOnUIElement(string GameObjectName, string Text, int fontSize = -1)
        {
            var elementRoot = GameObject.Find(GameObjectName);
            if (elementRoot == null) return;

            var uiElement = elementRoot.GetComponentInChildren<UILabel>();
            if (uiElement == null) return;

            elementRoot.GetComponentInChildren<UILabel>().text = Text;
            if (fontSize != -1)
                uiElement.fontSize = fontSize;
        }

        public static void SetColorOfUIElement(string GameObjectName, UnityEngine.Color color)
        {
            var elementRoot = GameObject.Find("DistanceTitle");
            if (elementRoot == null) return;

            var uiElement = elementRoot.GetComponentInChildren<UILabel>();
            if (uiElement == null) return;

            elementRoot.GetComponentInChildren<UILabel>().color = color;
        }
    }
}
