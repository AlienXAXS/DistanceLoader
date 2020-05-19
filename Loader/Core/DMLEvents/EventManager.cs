using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistanceLoader.Core.DMLEvents
{
    class EventManager
    {
        public static EventManager Instance = _instance ?? (_instance = new EventManager());
        private static readonly EventManager _instance;

        public event OnLocalCarCreatedEvent OnLocalCarCreated;
        public delegate void OnLocalCarCreatedEvent();

        public event OnLocalCarDestroyedEvent OnLocalCarDestroyed;
        public delegate void OnLocalCarDestroyedEvent();

        public enum DMLEvents
        {
            OnLocalCarCreated,
            OnLocalCarDestroyed,
            OnLocalCarReinitialized
        }

        public void FireEvent(DMLEvents eventToFire)
        {
            switch (eventToFire)
            {
                case DMLEvents.OnLocalCarCreated:
                    OnLocalCarCreated?.Invoke();
                    break;

                case DMLEvents.OnLocalCarDestroyed:
                    OnLocalCarDestroyed?.Invoke();
                    break;

            }
        }
    }
}
