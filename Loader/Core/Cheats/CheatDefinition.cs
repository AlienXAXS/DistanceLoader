using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistanceLoader.Core.Cheats
{
    interface CheatDefinition
    {
        List<InputAction> keyCombination { get; }
        string Name { get; }

        bool isActive { get; set; }
        
        void Start();
        void Stop();

        void Dispose();

    }
}
