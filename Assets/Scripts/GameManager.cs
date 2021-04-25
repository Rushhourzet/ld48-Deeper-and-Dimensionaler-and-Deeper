using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DaD {
    public class GameManager : MonoBehaviour {
        public AddressingSystem a_System;
        void Start() {
            a_System.GameStartEvent.Invoke();
        }

        // Update is called once per frame
        void Update() {

        }
    }
}
