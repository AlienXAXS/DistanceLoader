using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Exception = System.Exception;

namespace DistanceLoader.GUI.MainMenu
{
    public class Modifications
    {
        public static void AddButtonToMainMenu(string name, string title, EventDelegate.Callback buttonClickCallback, int order)
        {
            try
            {
                DistanceLoader.Util.Logger.Instance.Log($"[MainMenu-AddButtonToMainMenu] Adding new button to Main Menu {name}|{title}");

                var distanceLoaderMenuItem = new GameObject(name);
                distanceLoaderMenuItem.SetActive(false);
                distanceLoaderMenuItem.transform.SetParent(GameObject.Find("MainButtons").transform);
                distanceLoaderMenuItem.transform.SetSiblingIndex(order);
                distanceLoaderMenuItem.layer = 22;
                distanceLoaderMenuItem.transform.position += new Vector3(100, 100, 0);
                
                UIExButton newUiExButton = distanceLoaderMenuItem.AddComponent<UIExButton>();
                var distanceLoaderMenuButton = new EventDelegate(buttonClickCallback) {oneShot = false};
                if (!distanceLoaderMenuButton.isValid)
                {
                    // The button callback is not valid, this button will never work - destroy it
                    distanceLoaderMenuItem.Destroy();
                    return;
                }
                
                // Attaches our Callback event to the button
                newUiExButton.name = $"{name}_Button";
                newUiExButton.onClick = new List<EventDelegate>() {distanceLoaderMenuButton};
                newUiExButton.transform.SetParent(distanceLoaderMenuItem.transform);
                newUiExButton.SetState(UIButtonColor.State.Normal, true);
                newUiExButton.disabledColor = Color.grey;
                newUiExButton.defaultColor = Color.white;
                newUiExButton.SetButtonColor(Color.white);
                newUiExButton.enabled = true;

                // Not sure what this does, but it was needed in the original buttons
                BoxCollider newBoxCollider = distanceLoaderMenuItem.AddComponent<BoxCollider>();
                newBoxCollider.name = $"{name}_BoxCollider";
                newBoxCollider.transform.SetParent(distanceLoaderMenuItem.transform);
                newBoxCollider.size = new Vector3(205f, 36f, 0f);
                newBoxCollider.center = new Vector3(-1.2f, -0.2f, 0f);
                newBoxCollider.enabled = true;
                newBoxCollider.isTrigger = true;

                // Adds the ability to handle controller/keyboard menu navigation
                UIKeyNavigation newUiKeyNavigation = distanceLoaderMenuItem.AddComponent<UIKeyNavigation>();
                newUiKeyNavigation.name = $"{name}_KeyNav";
                newUiKeyNavigation.transform.SetParent(distanceLoaderMenuItem.transform);
                newUiKeyNavigation.startsSelected = false;

                // The label of the button, the actual text
                // This still makes the text purple after clicking it, unsure why yet
                UILabel newUiLabel = distanceLoaderMenuItem.AddComponent<UILabel>();
                newUiLabel.name = $"{name}_Label";
                newUiLabel.text = title;
                newUiLabel.fontSize = 32;
                newUiLabel.fontStyle = FontStyle.Normal;
                newUiLabel.color = Color.white;
                newUiLabel.effectColor = Color.blue;

                // Copies the font from the current "CAMPAIGN" button on the main menu.
                // Unity (according to forums) does not support creating new TrueType fonts during runtime, so we can just create a copy instead :)
                newUiLabel.trueTypeFont = GameObject.Find("MainButtons").GetChild(0).GetComponentInChildren<UILabel>().trueTypeFont; 
                newUiLabel.transform.SetParent(distanceLoaderMenuItem.transform);
                newUiLabel.ProcessText();
                newUiLabel.Update();

                // Activate the whole new GameObject
                distanceLoaderMenuItem.SetActive(true);
            }
            catch (Exception ex)
            {
                DistanceLoader.Util.Logger.Instance.Log("[MainMenu-AddButtonToMainMenu] Exception", ex);
            }
        }

    }
}
