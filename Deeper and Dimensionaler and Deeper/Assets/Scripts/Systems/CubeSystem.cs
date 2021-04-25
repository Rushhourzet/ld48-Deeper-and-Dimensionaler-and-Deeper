using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using static DaD.Cube;

namespace DaD {
    public class CubeSystem : MonoBehaviour {
        private readonly static int CHUNK_SIZE = 16;
        public Cube cubePrefab;
        private List<Cube> cubes;
        private List<Vector3> positionsLog;
        public Dictionary<CubeType, Texture> textures;
        public AddressingSystem a_System;
        private float cubeScale => cubePrefab.transform.localScale.x;
        public int playerDamage;

        public void Initialize() {
            a_System.playerDamageUpdatedAction = UpdatePlayerDamage;
            cubes = new List<Cube>((int)math.pow(CHUNK_SIZE, 2));
            positionsLog = new List<Vector3>((int)math.pow(CHUNK_SIZE, 2));
            for (int x = 0; x < CHUNK_SIZE; x++) {
                for (int z = 0; z < CHUNK_SIZE; z++) {
                    SpawnCube(cubePrefab, new Vector3(x * cubeScale, 0, z * cubeScale), AddCubeToList, false, 0);
                }
            }
            a_System.MapInitialized.Invoke();
        }

        public void DestroyCube(Cube cube, int overkillDamage) {
            Vector3 pos = cube.transform.position;
            if(pos.y != 0f) {
                SpawnCube(cubePrefab, new Vector3(pos.x - 1 * cubeScale, pos.y, pos.z), AddCubeToList, true, overkillDamage);
                SpawnCube(cubePrefab, new Vector3(pos.x + 1 * cubeScale, pos.y, pos.z), AddCubeToList, true, overkillDamage);
                SpawnCube(cubePrefab, new Vector3(pos.x, pos.y, pos.z - 1 * cubeScale), AddCubeToList, true, overkillDamage);
                SpawnCube(cubePrefab, new Vector3(pos.x, pos.y, pos.z + 1 * cubeScale), AddCubeToList, true, overkillDamage);
                SpawnCube(cubePrefab, new Vector3(pos.x, pos.y + 1 * cubeScale, pos.z), AddCubeToList, true, overkillDamage);
            }
            SpawnCube(cubePrefab, new Vector3(pos.x, pos.y - 1 * cubeScale, pos.z), AddCubeToList, true, overkillDamage);
            a_System.InvokeCubeDestroyedEvent(cube, a_System.cubeDestroyedAction);
            Destroy(cube.gameObject);
        }
        public void SpawnCube(Cube cube, Vector3 position, Action<Cube> addCubeToLists, bool checkIfPositionAvailable, int overkillDmg) {
            if (checkIfPositionAvailable) {
                foreach (var item in positionsLog) {
                    if (position == item) return;
                }
            }
            Cube tmp = Instantiate(cube, position, Quaternion.identity)
                        //.withID((ulong)((position.x + 1) * (position.z + 1) * (position.y + 1)))
                        .withSystem(this);
            addCubeToLists(tmp);
            tmp.TakeDamage(overkillDmg);
        }

        public void AddCubeToList(Cube cube) {
            positionsLog.Add(cube.transform.position);
            cubes.Add(cube);
        }

        public void UpdatePlayerDamage(int damage) {
            this.playerDamage = damage;
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
