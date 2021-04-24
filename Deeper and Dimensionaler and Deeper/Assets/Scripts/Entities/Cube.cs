using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DaD {
    public class Cube : MonoBehaviour {
        public CubeType type;
        public Vector3 position;

        public Cube(Vector3 position, CubeType type) {
            this.position = position;
            this.type = type;
        }
        public enum CubeType {
            dirt
        }
    }
}
