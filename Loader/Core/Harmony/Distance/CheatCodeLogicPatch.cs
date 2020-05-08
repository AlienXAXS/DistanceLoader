using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistanceLoader.Core.Harmony.Distance
{
    class CheatCodeLogicPatch
    { 
        private static bool lol = false;

        public static bool Update(ref List<CheatCodeLogic.CheatCode> ___allCheatCodes_, ref bool ___invalidateAll_)
        {
            var inputManager_ = G.Sys.InputManager_;
            var menuPanelManager_ = G.Sys.MenuPanelManager_;
            
            var allCheatCodes_ = ___allCheatCodes_;
            var invalidateAll_ = ___invalidateAll_;

            if (!inputManager_.GetKey(InputAction.MenuPageLeft, -2) || !inputManager_.GetKey(InputAction.MenuPageRight, -2) || menuPanelManager_.IsOneAboveRoot())
            {
                invalidateAll_ = true;
            }
            else
            {
                if (invalidateAll_)
                {
                    for (int index = 0; index < allCheatCodes_.Count; ++index)
                        allCheatCodes_[index].currentIndex_ = 0;
                    invalidateAll_ = false;
                }
                for (int index1 = 0; index1 < allCheatCodes_.Count; ++index1)
                {
                    CheatCodeLogic.CheatCode allCheatCode = allCheatCodes_[index1];
                    if (!allCheatCode.isUnlocked_)
                    {
                        InputAction action = allCheatCode.sequence_[allCheatCode.currentIndex_];
                        bool flag = false;
                        InputStates inputStates = G.Sys.InputManager_.GetInputStates(-1);
                        for (int index2 = 0; index2 < 55; ++index2)
                        {
                            if (inputStates.GetReleased((InputAction)index2))
                                flag = true;
                        }
                        if (flag)
                        {
                            if (inputManager_.GetKeyUp(action, -2))
                            {
                                ++allCheatCode.currentIndex_;
                                if (allCheatCode.currentIndex_ == allCheatCode.sequence_.Count)
                                {
                                    AudioManager.PostEvent("Play_VocalGPS");
                                    allCheatCode.onCheatCodeEntered_();
                                    allCheatCode.isUnlocked_ = true;
                                }
                            }
                            else
                                allCheatCode.currentIndex_ = 0;
                        }
                    }
                }
            }

            return false;
        }

        public static bool UnlockAllLevels()
        {
            Util.Logger.Instance.Log("YOU UNLOCKED ALL LEVELS!");
            return true;
        }
    }
}
