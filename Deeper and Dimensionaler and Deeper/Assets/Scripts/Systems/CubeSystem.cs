using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace DaD {
    public class CubeSystem : MonoBehaviour {
        private readonly static int CHUNK_SIZE = 32;
        public GameObject cubePrefab;
        public AddressingSystem a_System;

        public void Initialize() {
            for (int x = 0; x < CHUNK_SIZE; x++) {
                for (int z = 0; z < CHUNK_SIZE; z++) {
                    //for (int y = 0; y > -CHUNK_SIZE; y--) {}
                    Instantiate(cubePrefab, new Vector3(x, 0, z), Quaternion.identity);
                }
            }
            a_System.MapInitialized.Invoke();
        }

    }
}

//private ChunkData chunk;
//private CubeTypesNative cubeTypes;
//private CubeTypesObjects cubeTypesObj;
//private void Start() {
//    chunk.chunkSize = CHUNK_SIZE;
//    cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//}

//public void Initialize() {
//    var chunkGenJob = new ChunkGenerationJob {
//        chunk = new ChunkData {
//            positions = chunk.positions = new NativeArray<float>((int)math.pow(CHUNK_SIZE, 3), Allocator.Temp),
//            chunkSize = CHUNK_SIZE
//        }
//    };
//    JobHandle job = chunkGenJob.Schedule();

//    job.Complete();

//    for (int i = 0; i < chunkGenJob.length; i++) {
//        Instantiate(cube, new Vector3(i, i / CHUNK_SIZE + i % CHUNK_SIZE, i / (int)math.pow(CHUNK_SIZE, 2) + i % math.pow(CHUNK_SIZE, 2)), Quaternion.identity);
//    }

//    chunkGenJob.chunk.positions.Dispose();
//}


//[BurstCompile]
//private struct ChunkGenerationJob : IJob {
//    [WriteOnly]
//    public ChunkData chunk;
//    //[ReadOnly]
//    //public CubeTypesNative cubeTypesNat;
//    public int length;

//    public void Execute() {
//        chunk.chunkSize = CHUNK_SIZE;
//        length = (int)math.pow(chunk.chunkSize, 3);
//        for (int x = 0; x < chunk.chunkSize; x++) {
//            for (int y = 0; y < chunk.chunkSize; y++) {
//                for (int z = 0; z < chunk.chunkSize; z++) {
//                    chunk.positions[(x * chunk.chunkSize * chunk.chunkSize) + (y * chunk.chunkSize) + z] = z;
//                }
//            }
//        }
//        DebugLog(in chunk.positions);
//    }

//    [BurstDiscard]
//    static void DebugLog(in NativeArray<float> positions) {
//        Debug.Log($"coordinates are: {positions}");
//    }

//    public void Execute(int index) {
//        throw new System.NotImplementedException();
//    }
//}
