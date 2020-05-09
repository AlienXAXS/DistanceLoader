using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DistanceLoader.Core.Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace DistanceLoader.Core.Cheats
{
    class FlyHack : CheatDefinition
    {
        public List<InputAction> keyCombination => new List<InputAction>()
            {
                InputAction.MenuPageLeft,
                InputAction.MenuPageRight,
                InputAction.Wings
            };

        public string Name => "FlyHack";
        public bool isActive { get; set; }


        public void Start()
        {

            Util.Logger.Instance.Log("[MainThread] Attempting GUITester Label");
            //var uiRoot = GameObject.Find("DistanceTitle");
            //uiRoot.AddComponent<GUITester>();

            NGUIDebug.Log("Hello");

            //GameObject gameObject = new GameObject("GUITester");
            //Object.DontDestroyOnLoad(gameObject);
            //gameObject.AddComponent<GUITester>();

            Util.Logger.Instance.Log("[MainThread] Attempting GUITester Label - Complete");

            if (ApplicationEx.LoadedLevelName_.Equals("MainMenu") ||
                ApplicationEx.LoadedLevelName_.Equals("SplashScreens"))
                return;

            isActive = true;

            var cheatThread = new Thread(ThreadedHack){IsBackground = true};
            cheatThread.Start();
        }

        private void ThreadedHack()
        {
            var player = G.Sys.PlayerManager_.Current_;
            


            //playerVehicle.IgnoringInput_ = true;

            while (isActive)
            {
                var playerVehicle = player.playerData_.LocalCar_;

                if (playerVehicle != null)
                {
                    var playerPosition = new Vector3(playerVehicle.transform.position.x,
                        playerVehicle.transform.position.y, playerVehicle.transform.position.z);

                    if (G.Sys.InputManager_.GetKey(InputAction.SteerLeft))
                    {
                        playerPosition.Set(playerPosition.x - 20, playerPosition.y, playerPosition.z);
                        playerVehicle.transform.position = playerPosition; //Update the position
                    }

                    if (G.Sys.InputManager_.GetKey(InputAction.SteerRight))
                    {
                        playerPosition.Set(playerPosition.x + 20, playerPosition.y, playerPosition.z);
                        //playerVehicle.transform.position = playerPosition; //Update the position
                    }

                    PlayerDataLocal.FirstLocalPlayer_.ResetPosition_.Set(playerPosition.x, playerPosition.y,
                        playerPosition.z);
                }

                Thread.Sleep(30);
            }
        }

        public void Stop()
        {
            isActive = false;
            G.Sys.PlayerManager_.Current_.playerData_.LocalCar_.IgnoringInput_ = false;
        }

        public void Dispose()
        {}
    }
}
