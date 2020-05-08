using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

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
            if (ApplicationEx.LoadedLevelName_.Equals("MainMenu") ||
                ApplicationEx.LoadedLevelName_.Equals("SplashScreens"))
                return;

            isActive = true;

            var cheatThread = new Thread(ThreadedHack){IsBackground = true};
            cheatThread.Start();
        }

        private void ThreadedHack()
        {
            while (isActive)
            {
                var player = G.Sys.PlayerManager_.Current_;
                var playerVehicle = player.playerData_.LocalCar_;
                var playerPosition = new Vector3(playerVehicle.transform.position.x, playerVehicle.transform.position.y, playerVehicle.transform.position.z);

                if (G.Sys.InputManager_.GetKey(InputAction.SteerLeft))
                {
                    playerPosition.Set(playerPosition.x - 20, playerPosition.y, playerPosition.z);
                    playerVehicle.transform.position = playerPosition; //Update the position
                }

                if (G.Sys.InputManager_.GetKey(InputAction.SteerRight))
                {
                    playerPosition.Set(playerPosition.x + 20, playerPosition.y, playerPosition.z);
                    playerVehicle.transform.position = playerPosition; //Update the position
                }

                Thread.Sleep(30);
            }
        }

        public void Stop()
        {
            isActive = false;
        }

        public void Dispose()
        {}
    }
}
