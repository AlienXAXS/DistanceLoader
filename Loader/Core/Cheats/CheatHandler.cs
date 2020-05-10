using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DistanceLoader.Core.Cheats
{
    class CheatHandler : IDisposable
    {
        private readonly List<CheatDefinition> cheatList = new List<CheatDefinition>();
        private bool isShutdown = false;
        private string lastLevelName = "";

        public void LoadCheats()
        {
            if (DistanceLoader.Util.Configuration.Instance.IsDebug)
                DebugMode();

            // Load our cheats
            cheatList.Add(new FlyHack());

            Util.Logger.Instance.Log($"[CheatHandler] I have {cheatList.Count} cheats ready to fire!");

            var cheatDetectionThread = Util.ThreadManager.Instance.CreateNewThread(DetectCheat);
            cheatDetectionThread.Start();

            var waitForPlayerThread = Util.ThreadManager.Instance.CreateNewThread(WaitForPlayer);
            waitForPlayerThread.Start();

        }

        private void WaitForPlayer()
        {
            Util.Logger.Instance.Log("[HealthHack-WaitForPlayer] Waiting for player car to be ready.");
            while (G.Sys.PlayerManager_?.Current_?.playerData_?.LocalCar_ == null)
            {
                Thread.Sleep(1000);
            }
            Util.Logger.Instance.Log("[HealthHack-WaitForPlayer] Player vehicle found");

            // Add monobehavour cheats
            GameObject gameObject = new GameObject("HealthHack");
            UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object)gameObject);
            gameObject.AddComponent<HealthHack>();
        }

        private void DetectCheat()
        {
            var inputManager = G.Sys.InputManager_;
            var cheatCanActive = false;
            var expectKeyUp = false;

            while (!isShutdown)
            {
                foreach (var cheat in cheatList)
                {
                    if (!expectKeyUp)
                    {
                        var pressedKeyCount = 0;
                        foreach (var key in cheat.keyCombination)
                        {
                            if ( inputManager.GetKey(key) )
                                pressedKeyCount++;
                        }

                        if (pressedKeyCount == cheat.keyCombination.Count)
                        {
                            cheatCanActive = true;
                            expectKeyUp = true;
                        }
                    }
                    else
                    {
                        InputStates inputStates = G.Sys.InputManager_.GetInputStates(-1);
                        foreach (var key in cheat.keyCombination)
                        {
                            if (inputStates.GetReleased(key))
                            {
                                expectKeyUp = false;
                            };
                        }

                        if (expectKeyUp == false && cheatCanActive)
                        {
                            AudioManager.PostEvent("Play_VocalGPS");
                            switch (cheat.isActive)
                            {
                                case true:
                                    Util.Logger.Instance.Log($"[CheatHandler] Cheat {cheat.Name} Stopping");
                                    cheat.Stop();
                                    break;
                                case false:
                                    Util.Logger.Instance.Log($"[CheatHandler] Cheat {cheat.Name} Starting");
                                    cheat.Start();
                                    break;
                            }

                            cheatCanActive = false;
                        }
                    }
                }
            }
        }

        public void DebugMode()
        {
            Util.Logger.Instance.Log("[CheatHandler] Debug Mode Activating");
            var debugThread = new Thread(DebugThread) {IsBackground = true};
            debugThread.Start();
            Util.Logger.Instance.Log("[CheatHandler] Debug Mode Activated");
        }

        private void DebugThread()
        {
            Util.Logger.Instance.Log("[CheatHandler] Debug Thread Started");
            while (!isShutdown)
            {
                if (lastLevelName != ApplicationEx.LoadedLevelName_)
                {
                    Util.Logger.Instance.Log($"[CheatHandler-Debug] Loaded Level Change from {lastLevelName} to {ApplicationEx.LoadedLevelName_}");
                    lastLevelName = ApplicationEx.LoadedLevelName_;
                }

                Thread.Sleep(1000);
            }
            Util.Logger.Instance.Log("[CheatHandler] Debug Thread Ended");
        }

        public void Dispose()
        {
            isShutdown = true;
        }
    }
}
