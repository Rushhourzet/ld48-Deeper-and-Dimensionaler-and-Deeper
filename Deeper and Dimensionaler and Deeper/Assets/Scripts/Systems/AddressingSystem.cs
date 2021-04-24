using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DaD {
    public class AddressingSystem : MonoBehaviour {
        public UnityEvent GameStartEvent;
        public UnityEvent MapInitialized;
        public UnityEvent PlayerInitialized;
    }
}
