using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;

namespace DaD {
    public struct CubeTypesNative {
        public int size;
        public NativeArray<int> maxHealth;
        public NativeArray<int> value;
    }
}
