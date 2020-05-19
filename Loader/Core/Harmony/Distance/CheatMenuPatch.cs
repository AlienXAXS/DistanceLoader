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
        }

        public static bool CheatMenuPre(ref CheatMenu __instance)
        {
            if (__instance.name != "CheatsMenu") return true;

            Type typeInQuestion = typeof(CheatMenu);
            var newMenuObject = UIExBlueprint.Duplicate(__instance.menuBlueprint_);
            FieldInfo menuObjectField = typeInQuestion.BaseType.GetField("menuObject_", BindingFlags.Instance | BindingFlags.NonPublic);
            if (menuObjectField == null)
            {
                Util.Logger.Instance.Log($"[CheatMenuPre] {__instance.name} menuObject_ field get fail");
                return true;
            }

            menuObjectField.SetValue(__instance, newMenuObject);
            SuperDuperMenu newMenu = newMenuObject.GetComponent<SuperDuperMenu>();
            FieldInfo menuField = typeInQuestion.BaseType.GetField("menu_", BindingFlags.NonPublic | BindingFlags.Instance);

            menuField.SetValue(__instance, newMenu);

            if (newMenu == null)
            {
                Util.Logger.Instance.Log($"[CheatMenuPre] {__instance.name}  NewMenu is null (oh shit!)");
                return false;
            }

            newMenu.Init(__instance);
            newMenu.menuTitleLabel_.text = "PLZ NO CHEAT :D";
            newMenuObject.name = __instance.Name_;

            return false;
        }
    }
}
