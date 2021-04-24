using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Mathematics;

namespace DaD {
    public struct ChunkData {
        public int chunkSize;
        public Cubes cubes;
        public NativeArray<bool> isActive;
        public NativeArray<bool> isAlive;

    }
}
