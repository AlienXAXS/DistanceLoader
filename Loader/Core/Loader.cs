using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;

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
                if (File.Exists(Path.Combine(appData, "DEBUG_MODE"))) ;
                {

                    Util.Logger.Instance.Log($"[MainThread] Waiting for main menu");
                    //while (ApplicationEx.LoadedLevelName_ != "MainMenu")
                    //    Thread.Sleep(500);

                    Util.Logger.Instance.Log($"[MainThread] Main Menu Detected!");

                    Util.Logger.Instance.Log($"[MainThread] Creating new patchmanager");
                    var harmonyDistancePatcher = new DistanceLoader.Core.Harmony.PatchManager();

                    Util.Logger.Instance.Log($"[MainThread] Starting patching");
                    harmonyDistancePatcher.BeginPatching();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void Init()
        {
            var bgWorker = new Thread(MainThread) {IsBackground = true};
            bgWorker.Start();
        }
    }
}
