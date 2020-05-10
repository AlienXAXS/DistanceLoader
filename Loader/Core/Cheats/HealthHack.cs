using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Events;
using Events.Car;
using Events.Player;
using UnityEngine;

namespace DistanceLoader.Core.Cheats
{
    class HealthHack : MonoBehaviour
    {
        private PlayerEvents playerEvents_;
        private PlayerDataLocal playerData_;
        private CarLogic carLogic_;


        /*
         * TODO:
         This breaks when the player leaves for the main menu as the car object is destroyed.
         Possible to maybe hook into 'StoryIntroCutsceneLogic', which is responsible for spawning the car.
         */

        private void OnEnable()
        {
            Util.Logger.Instance.Log("[HealthHack-OnEnable] - OnEnable, hooking events");

            this.playerData_ = G.Sys.PlayerManager_.Current_.playerData_;
            if (this.playerData_ == null)
            {
                Util.Logger.Instance.Log("[HealthHack-OnEnable] playerData is null, cannot keep going!");
                return;
            }

            this.playerEvents_ = this.playerData_.Events_;
            this.playerEvents_.SubscribeToCarInstantiateHelper(new InstancedEvent<CarInstantiate.Data>.Delegate(this.OnPlayerEventCarInstantiate));
            this.playerEvents_.Subscribe<Impact.Data>(new InstancedEvent<Impact.Data>.Delegate(this.OnCarEventImpact));
            this.playerEvents_.Subscribe<Death.Data>(new InstancedEvent<Death.Data>.Delegate(this.OnCarEventDeath));
            this.playerEvents_.Subscribe<Split.Data>(new InstancedEvent<Split.Data>.Delegate(this.OnCarEventSplit));
        }

        private void OnDisable()
        {
            this.playerEvents_.UnsubscribeToCarInstantiateHelper(new InstancedEvent<CarInstantiate.Data>.Delegate(this.OnPlayerEventCarInstantiate));
            this.playerEvents_.Unsubscribe<Impact.Data>(new InstancedEvent<Impact.Data>.Delegate(this.OnCarEventImpact));
            this.playerEvents_.Unsubscribe<Death.Data>(new InstancedEvent<Death.Data>.Delegate(this.OnCarEventDeath));
            this.playerEvents_.Unsubscribe<Split.Data>(new InstancedEvent<Split.Data>.Delegate(this.OnCarEventSplit));
        }

        private void OnCarEventSplit(Split.Data data)
        {
            
        }

        private void OnCarEventDeath(Death.Data data)
        {
            Util.Logger.Instance.Log($"[HealthHack] Car Died! {data.causeOfDeath}");
        }

        private void OnCarEventImpact(Impact.Data data)
        {
            Util.Logger.Instance.Log($"[HealthHack] Car Impact! speed:{data.speed_} with:{data.impactedCollider_.name}");
        }

        private void OnPlayerEventCarInstantiate(CarInstantiate.Data data)
        {
            this.carLogic_ = data.car.GetComponent<CarLogic>();
            this.carLogic_.Boost_.accelerationMul_ = 1.25f;
            this.carLogic_.infiniteCooldown_ = true;
        }
    }
}
