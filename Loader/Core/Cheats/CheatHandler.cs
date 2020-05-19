using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DistanceLoader.Core.Cheats
{
    public class CheatMemory
    {
        public bool LasersNoDamage = false;
        public bool InfiniteBoost = false;
        public bool AlwaysEnableBoost = false;
        public bool AlwaysEnableJump = false;
        public bool AlwaysEnableWings = false;
        public bool AlwaysEnableJets = false;
    }

    public class CheatHandler
    {
        private readonly List<CheatDefinition> cheatList = new List<CheatDefinition>();
        private string lastLevelName = "";

        public static CheatHandler Instance = _instance ?? (_instance = new CheatHandler());
        private static readonly CheatHandler _instance;

        private const string CheatMemoryFilePath = @".\DistanceLoader\Cheats.json";

        public CheatMemory Cheats = new CheatMemory();

        public void LoadCheats()
        {
            // Load our cheats
            //cheatList.Add(new FlyHack());

            Util.Logger.Instance.Log("[CheatHandler] Loading cheat status from JSON file");

            if (System.IO.File.Exists(CheatMemoryFilePath))
            {
                CheatMemory cheatMemory =
                    (CheatMemory) JsonUtility.FromJson(System.IO.File.ReadAllText(CheatMemoryFilePath),
                        typeof(CheatMemory));
                Cheats = cheatMemory;
            }
            else
            {
                System.IO.File.WriteAllText(CheatMemoryFilePath, JsonUtility.ToJson(Cheats, true));
            }

            Util.Logger.Instance.Log($"[CheatHandler] I have {cheatList.Count} cheats ready to fire!");
            var cheatDetectionThread = Util.ThreadManager.Instance.CreateNewThread(DetectCheat);
            cheatDetectionThread.Start();

            DMLEvents.EventManager.Instance.OnLocalCarCreated += OnLocalCarCreated;
        }

        private void OnLocalCarCreated()
        {
            var localCar = G.Sys.PlayerManager_.Current_.playerData_.LocalCar_;

            if (Cheats.InfiniteBoost)
            {
                var carLogic = localCar.gameObject.GetComponent<CarLogic>();
                carLogic.infiniteCooldown_ = true;
                carLogic.Boost_.accelerationMul_ = 1.15f;
                Util.Logger.Instance.Log($"[CheatHandler-CheatActivated] Infinite Boost Energy");
            }

            if (Cheats.AlwaysEnableBoost)
            {
                var carBoostObject = localCar.gameObject.GetComponentWithGameObjectNullCheck<BoostGadget>();
                if (carBoostObject != null)
                {
                    Util.Logger.Instance.Log("[CheatHandler-CheatActivated] Always Enable Boost Gadget");
                    carBoostObject.SetAbilityEnabled(true, true);
                }
            }

            if ( Cheats.AlwaysEnableJets )
            {
                // Can do it this way too
                //localCar.PlayerDataLocal_.CarLogic_.Wings_.SetAbilityEnabled(true,true);

                var carJetObject = localCar.gameObject.GetComponentWithGameObjectNullCheck<JetsGadget>();
                if (carJetObject != null)
                {
                    Util.Logger.Instance.Log("[CheatHandler-CheatActivated] Always Enable Jet Gadget");
                    carJetObject.SetAbilityEnabled(true, true);
                }
            }

            if (Cheats.AlwaysEnableJump)
            {
                var carJumpObject = localCar.gameObject.GetComponentWithGameObjectNullCheck<JumpGadget>();
                if (carJumpObject != null)
                {
                    Util.Logger.Instance.Log("[CheatHandler-CheatActivated] Always Enable Jump Gadget");
                    carJumpObject.SetAbilityEnabled(true, true);
                }
            }

            if (Cheats.AlwaysEnableWings)
            {
                var carWingsObject = localCar.gameObject.GetComponentWithGameObjectNullCheck<WingsGadget>();
                if (carWingsObject != null)
                {
                    Util.Logger.Instance.Log("[CheatHandler-CheatActivated] Always Enable Wings Gadget");
                    carWingsObject.SetAbilityEnabled(true, true);
                }
            }

            if (Cheats.LasersNoDamage)
            {
                Util.Logger.Instance.Log("[CheatHandler-CheatActivated] Lasers No Damage");
            }
        }

        private void DetectCheat()
        {
            Util.Logger.Instance.Log("[CheatHandler-DetectCheat] Starting Cheat Key Input Detection!");

            var inputManager = G.Sys.InputManager_;
            var cheatCanActive = false;
            var expectKeyUp = false;
            
            while (!Util.ThreadManager.Instance.GameShutdownInitiated)
            {
                try
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
                                        Util.Logger.Instance.Log($"[CheatHandler-DetectCheat] Cheat {cheat.Name} Stopping");
                                        cheat.Stop();
                                        break;
                                    case false:
                                        Util.Logger.Instance.Log($"[CheatHandler-DetectCheat] Cheat {cheat.Name} Starting");
                                        cheat.Start();
                                        break;
                                }

                                cheatCanActive = false;
                            }
                        }
                    }
                    Thread.Sleep(500);
                }
                catch (ThreadAbortException)
                {
                    Util.Logger.Instance.Log("[CheatHandler-DetectCheat] Thread Aborted");
                    return;
                }
                catch (Exception ex)
                {
                    Util.Logger.Instance.Log("[CheatHandler-DetectCheat] Exception", ex);
                    return;
                }
            }
        }
    }
}
