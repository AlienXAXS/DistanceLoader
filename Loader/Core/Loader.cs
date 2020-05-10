﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using DistanceLoader.Core.Harmony;
using DistanceLoader.Core.Harmony.GUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using System.Linq;
using System.Net.Mime;
using Component = UnityEngine.Component;

namespace DistanceLoader.Loader.Core
{
    public class Loader
    {
        public void MainThread()
        {
            Util.Logger.Instance.Log("DML Main Thread Started");

            var appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DistanceLoader");
            try
            {
                // Creates our staging area in %APPDATA\DistanceLoader
                Directory.CreateDirectory(appData);
                
                // Alloc a new console for us, to output random guff to
                if (File.Exists(Path.Combine(appData, "DEBUG_MODE")))
                {
                    Util.Logger.Instance.Log("DML Is loading in Debug Mode!");
                    Util.Configuration.Instance.IsDebug = true;
                }

                Util.Logger.Instance.Log($"[MainThread] Creating new patchmanager");
                var harmonyDistancePatcher = new DistanceLoader.Core.Harmony.PatchManager();

                Util.Logger.Instance.Log($"[MainThread] Starting patching");
                harmonyDistancePatcher.BeginPatching();

                Util.Logger.Instance.Log("[MainThread] Starting Cheat Engine");
                var cheatEngine = new DistanceLoader.Core.Cheats.CheatHandler();
                cheatEngine.LoadCheats();

                SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
                SceneManager.activeSceneChanged += SceneManagerOnactiveSceneChanged;
                SceneManager.sceneUnloaded += SceneManagerOnsceneUnloaded;
            }
            catch (Exception ex)
            {

            }
        }

        public void TestMethod()
        {
            Util.Logger.Instance.Log("CLICKY CLICKY");
        }

        private void SceneManagerOnactiveSceneChanged(Scene prevScene, Scene newScene)
        {
            try
            {
                Util.Logger.Instance.Log($"[SceneManagerOnactiveSceneChanged] {prevScene.name} -> {newScene.name} (IsLoaded:{newScene.isLoaded})");
                if (newScene.isLoaded)
                {
                    if (newScene.name != "MainMenu")
                        return;

                    Util.Logger.Instance.Log($"[SceneManagerOnactiveSceneChanged] Starting MainMenu modifications");

                    //var objects = newScene.GetRootGameObjects();
                    //DumpGameObjects(objects);

                    var VersionNumberLabel = GameObject.Find("DistanceTitle");
                    if (VersionNumberLabel != null)
                    {
                        VersionNumberLabel.GetComponentInChildren<UILabel>().text="DISTANCE MOD LOADER";
                        VersionNumberLabel.GetComponentInChildren<UILabel>().fontSize = 46;
                    }

                    var distanceLoaderMenuItem = new GameObject();
                    distanceLoaderMenuItem.name = "DistanceLoaderMenuItem";
                    distanceLoaderMenuItem.SetActive(false);
                    distanceLoaderMenuItem.transform.SetParent(GameObject.Find("MainButtons").transform);
                    distanceLoaderMenuItem.transform.SetSiblingIndex(1);
                    distanceLoaderMenuItem.layer = 22;
                    distanceLoaderMenuItem.transform.position += new Vector3(100, 100, 0);

                    UIExButton newUiExButton = distanceLoaderMenuItem.AddComponent<UIExButton>();
                    var distanceLoaderMenuLogic = new DistanceLoaderMenuLogic();
                    var distanceLoaderMenuButton = new EventDelegate(distanceLoaderMenuLogic.OnClick) {oneShot = false};
                    Util.Logger.Instance.Log($"Button Valid? {distanceLoaderMenuButton.isValid} | {distanceLoaderMenuButton.methodName}");

                    newUiExButton.onClick = new List<EventDelegate>(){ distanceLoaderMenuButton };
                    newUiExButton.transform.SetParent(distanceLoaderMenuItem.transform);
                    newUiExButton.SetState(UIButtonColor.State.Normal, true);
                    newUiExButton.disabledColor = Color.grey;
                    newUiExButton.defaultColor = Color.white;
                    newUiExButton.SetButtonColor(Color.white);
                    newUiExButton.enabled = true;


                    BoxCollider newBoxCollider = distanceLoaderMenuItem.AddComponent<BoxCollider>(); 
                    newBoxCollider.transform.SetParent(distanceLoaderMenuItem.transform);
                    newBoxCollider.size = new Vector3(205f, 36f, 0f);
                    newBoxCollider.center = new Vector3(-1.2f, -0.2f, 0f);
                    newBoxCollider.enabled = true;
                    newBoxCollider.isTrigger = true;


                    UIKeyNavigation newUiKeyNavigation = distanceLoaderMenuItem.AddComponent<UIKeyNavigation>();
                    newUiKeyNavigation.transform.SetParent(distanceLoaderMenuItem.transform);
                    newUiKeyNavigation.startsSelected = false;
                    newUiKeyNavigation.name = "DistanceLoader";
                    

                    UILabel newUiLabel = distanceLoaderMenuItem.AddComponent<UILabel>();
                    newUiLabel.name = "CustomLabelAGN";
                    newUiLabel.text = "Distance Mod Loader Settings";
                    newUiLabel.fontStyle = FontStyle.Italic;
                    newUiLabel.fontSize = 32;
                    newUiLabel.fontStyle = FontStyle.Normal;
                    newUiLabel.color = Color.white;
                    newUiLabel.ProcessText();
                    newUiLabel.Update();
                    newUiLabel.trueTypeFont = GameObject.Find("MainButtons").GetChild(0).GetComponentInChildren<UILabel>().trueTypeFont;
                    newUiLabel.transform.SetParent(distanceLoaderMenuItem.transform);
                    distanceLoaderMenuItem.SetActive(true);

                    var MainMenuButtons_Campaign = GameObject.Find("MainButtons").GetChild(0);
                    UILabel _uiLabel = MainMenuButtons_Campaign.GetComponentInChildren<UILabel>();
                    _uiLabel.color = Color.green;

                    Util.Logger.Instance.Log("#################################################################################### NEW MENU ITEM");
                    //Util.ObjectDumper.DumpGameObjects(distanceLoaderMenuItem, false);
                    Util.Logger.Instance.Log("#################################################################################### END");

                }
            }
            catch (Exception ex)
            {
                Util.Logger.Instance.Log($"[SceneManagerOnactiveSceneChanged] EXCEPTION", ex);
            }

        }

        private void OnRender(Material mat)
        {
            //Util.Logger.Instance.Log($"[CampaignUILabel-OnRender] {mat.name}");
        }

        private void SceneManagerOnsceneUnloaded(Scene scene)
        {
            Util.Logger.Instance.Log($"[SceneManagerOnsceneUnloaded] -> NAME: {scene.name}");
        }

        private void SceneManagerOnsceneLoaded(Scene scene, LoadSceneMode mode)
        {
            try
            {
                Util.Logger.Instance.Log($"[SceneManagerOnsceneLoaded] -> NAME: {scene.name}");
                if (scene.isLoaded)
                {
                    var objects = scene.GetRootGameObjects();

                    Util.Logger.Instance.Log(
                        $"[SceneManagerOnsceneLoaded] -> GetRootGameObjects Count: {objects.Length}");
                    Util.Logger.Instance.Log($"[SceneManagerOnsceneLoaded] -> GetRootGameObjects: {objects}");

                    Util.Logger.Instance.Log($"[SceneManagerOnsceneLoaded] -> {Util.ObjectDumper.Dump(scene)}");
                }
                else
                {
                    Util.Logger.Instance.Log($"[SceneManagerOnsceneLoaded] Scene {scene.name} is not loaded - but just got loaded?");
                }
            }
            catch (Exception ex)
            {
                Util.Logger.Instance.Log($"[SceneManagerOnsceneLoaded] Exception", ex);
            }
        }

        public void Init()
        {
            var bgWorker = new Thread(MainThread) {IsBackground = true};
            bgWorker.Start();
        }
    }
}
