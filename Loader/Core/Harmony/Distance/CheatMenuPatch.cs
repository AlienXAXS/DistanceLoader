using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace DistanceLoader.Core.Harmony.Distance
{
    public class CheatMenuPatch
    {
        public static void AwakePost(ref CheatMenu __instance)
        {
            Util.Logger.Instance.Log($"[CheatMenu - Awake Patch] Awake from {__instance.PanelObject_.name}");
        }

        public static bool CheatMenuPre(ref CheatMenu __instance)
        {
            if (__instance.name != "CheatsMenu") return true;

            Type typeInQuestion = typeof(CheatMenu);

            Util.Logger.Instance.Log($"[CheatMenuPre] {__instance.name} Creating a menu");

            var newMenuObject = UIExBlueprint.Duplicate(__instance.menuBlueprint_);

            Util.Logger.Instance.Log($"[CheatMenuPre] {__instance.name} 1");

            FieldInfo menuObjectField = typeInQuestion.BaseType.GetField("menuObject_", BindingFlags.Instance | BindingFlags.NonPublic);
            if (menuObjectField == null)
            {
                Util.Logger.Instance.Log($"[CheatMenuPre] {__instance.name} menuObject_ field get fail");
                return true;
            }
            Util.Logger.Instance.Log($"[CheatMenuPre] {__instance.name} 2");
            menuObjectField.SetValue(__instance, newMenuObject);
            Util.Logger.Instance.Log($"[CheatMenuPre] {__instance.name} 3");

            SuperDuperMenu newMenu = newMenuObject.GetComponent<SuperDuperMenu>();
            Util.Logger.Instance.Log($"[CheatMenuPre] {__instance.name} 4");
            FieldInfo menuField = typeInQuestion.BaseType.GetField("menu_", BindingFlags.NonPublic | BindingFlags.Instance);
            Util.Logger.Instance.Log($"[CheatMenuPre] {__instance.name} 5");
            menuField.SetValue(__instance, newMenu);
            Util.Logger.Instance.Log($"[CheatMenuPre] {__instance.name} 6");
            if (newMenu == null)
            {
                Util.Logger.Instance.Log($"[CheatMenuPre] {__instance.name}  NewMenu is null (oh shit!)");
                return false;
            }

            Util.Logger.Instance.Log($"[CheatMenuPre] {__instance.name} Init Menu!");
            newMenu.Init(__instance);
            newMenu.menuTitleLabel_.text = "PLZ NO CHEAT :D";
            newMenuObject.name = __instance.Name_;

            return false;
        }
    }
}
