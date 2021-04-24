using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DaD {
    public class AddressingSystem : MonoBehaviour {
        public UnityEvent GameStartEvent;
        public UnityEvent MapInitialized;
        public UnityEvent PlayerInitialized;
        public UnityEvent PlayerMoneyUpdated;
        public delegate void CubeDestroyedEvent(Cube cube, CubeDestroyedAction action);
        public delegate void CubeDestroyedAction(Cube cube);

        //public CubeDestroyedEvent cubeDestroyedEvent;
        public CubeDestroyedAction cubeDestroyedAction;

        public void InvokeCubeDestroyedEvent(Cube cube, CubeDestroyedAction action) {
            action(cube);
        }

    }
}
