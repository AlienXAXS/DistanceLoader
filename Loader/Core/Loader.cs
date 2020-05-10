using System;
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
                    if (newScene.name == "MainMenu")
                    {
                        Util.Logger.Instance.Log($"[SceneManagerOnactiveSceneChanged] Starting MainMenu modifications");
                        DistanceLoader.GUI.Util.SetTextOnUIElement("DistanceTitle", "Distance Mod Loader", 46);

                        DistanceLoader.GUI.MainMenu.Modifications.AddButtonToMainMenu("DistanceLoaderSettings",
                            "Distance Loader Settings", new DistanceLoaderMenuLogic().OnClick, 98);

                        DistanceLoader.GUI.MainMenu.Modifications.AddButtonToMainMenu("AboutDistanceLoader",
                            "About Distance Loader", new DistanceLoaderMenuLogic().OnClick, 99);
                    }


                    Util.ObjectDumper.DumpGameObjects(newScene.GetRootGameObjects(), 2);

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
