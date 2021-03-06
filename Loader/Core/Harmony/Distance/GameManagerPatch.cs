﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace DistanceLoader.Core.Harmony.Distance
{
    public class GameManagerPatch
    {
        public static void QuitGame()
        {
            Util.Logger.Instance.Log("Attempting to kill this game");
            Util.ThreadManager.Instance.KillAllThreads();
            Util.ThreadManager.Instance.GameShutdownInitiated = true;
        }
    }
}
