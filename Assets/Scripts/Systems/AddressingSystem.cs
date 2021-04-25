using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DaD {
    public class AddressingSystem : MonoBehaviour {
        public PlayerSystem p_system;
        public UnityEvent GameStartEvent;
        public UnityEvent MapInitialized;
        public UnityEvent PlayerInitialized;
        public UnityEvent PlayerMoneyUpdated;
        public delegate void CubeDestroyedEvent(Cube cube, CubeDestroyedAction action);
        public delegate void CubeDestroyedAction(Cube cube);
        public delegate void PlayerDamageUpdatedEvent(int damage, PlayerDamageUpdatedAction action);
        public delegate void PlayerDamageUpdatedAction(int damage);

        public PlayerDamageUpdatedAction playerDamageUpdatedAction;
        public PlayerDamageUpdatedEvent playerDamageTakenEvent;
        public void PlayerDamageUpdated(int damage, PlayerDamageUpdatedAction action) {
            action(damage);
        }

        //public CubeDestroyedEvent cubeDestroyedEvent;

        public void InvokeCubeDestroyedEvent(Cube cube) {
            p_system.UpdatePlayerMoneyWhenCubeGetsDestroyed(cube);
        }

    }
}
