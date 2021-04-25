using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using static DaD.Cube;
using Random = UnityEngine.Random;

namespace DaD {
    public class CubeSystem : MonoBehaviour {
        private readonly static int CHUNK_SIZE = 16;
        public Cube cubePrefab;
        private List<Cube> cubes;
        private List<Vector3> positionsLog;
        public Dictionary<CubeType, Texture> textures;
        public AddressingSystem a_System;
        public Material[] materials;

        public List<SpawnCubeCommand> spawnCubeCommandBuffer;

        private float cubeScale => cubePrefab.transform.localScale.x;
        public int playerDamage;

        private void Update() {
            if (spawnCubeCommandBuffer.Count > 0) {
                SpawnCubes(ref spawnCubeCommandBuffer);
            }
        }
        public void Initialize() {
            spawnCubeCommandBuffer = new List<SpawnCubeCommand>();
            a_System.playerDamageUpdatedAction = UpdatePlayerDamage;
            cubes = new List<Cube>((int)math.pow(CHUNK_SIZE, 2));
            positionsLog = new List<Vector3>((int)math.pow(CHUNK_SIZE, 2));
            for (int x = 0; x < CHUNK_SIZE; x++) {
                for (int z = 0; z < CHUNK_SIZE; z++) {
                    for (int y = 0; y < CHUNK_SIZE; y++) {
                        if (y > 0) {
                            //blocking spawn area
                            positionsLog.Add(new Vector3(x * cubeScale, y * cubeScale, z * cubeScale));
                        }
                    }
                    spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(x * cubeScale, 0, z * cubeScale), AddCubeToList, false, 0));

                }
            }
            a_System.MapInitialized.Invoke();
        }

        public void DestroyCube(Cube cube, Vector3 pos, int overkillDamage) {
            if (cube != null) {
                a_System.InvokeCubeDestroyedEvent(cube);
                Destroy(cube.gameObject);
            }
            int primaryDamage = overkillDamage / 3;
            //spawn direct neighbours - primary spawn
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x - 1 * cubeScale, pos.y, pos.z), AddCubeToList, true, overkillDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x + 1 * cubeScale, pos.y, pos.z), AddCubeToList, true, overkillDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x, pos.y, pos.z - 1 * cubeScale), AddCubeToList, true, overkillDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x, pos.y, pos.z + 1 * cubeScale), AddCubeToList, true, overkillDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x, pos.y + 1 * cubeScale, pos.z), AddCubeToList, true, overkillDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x, pos.y - 1 * cubeScale, pos.z), AddCubeToList, true, overkillDamage));
            //spawn Diagonally - secondary spawn
            int secondaryDamage = overkillDamage / 4;
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x - 1 * cubeScale, pos.y, pos.z - 1 * cubeScale), AddCubeToList, true, secondaryDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x - 1 * cubeScale, pos.y, pos.z + 1 * cubeScale), AddCubeToList, true, secondaryDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x + 1 * cubeScale, pos.y, pos.z - 1 * cubeScale), AddCubeToList, true, secondaryDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x + 1 * cubeScale, pos.y, pos.z + 1 * cubeScale), AddCubeToList, true, secondaryDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x, pos.y + 1 * cubeScale, pos.z - 1 * cubeScale), AddCubeToList, true, secondaryDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x, pos.y + 1 * cubeScale, pos.z + 1 * cubeScale), AddCubeToList, true, secondaryDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x, pos.y - 1 * cubeScale, pos.z - 1 * cubeScale), AddCubeToList, true, secondaryDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x, pos.y - 1 * cubeScale, pos.z + 1 * cubeScale), AddCubeToList, true, secondaryDamage));
            //tertiary spawn
            int tertiaryDamage = overkillDamage / 5;
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x - 1 * cubeScale, pos.y + 1 * cubeScale, pos.z - 1 * cubeScale), AddCubeToList, true, tertiaryDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x - 1 * cubeScale, pos.y + 1 * cubeScale, pos.z + 1 * cubeScale), AddCubeToList, true, tertiaryDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x + 1 * cubeScale, pos.y - 1 * cubeScale, pos.z - 1 * cubeScale), AddCubeToList, true, tertiaryDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x + 1 * cubeScale, pos.y - 1 * cubeScale, pos.z + 1 * cubeScale), AddCubeToList, true, tertiaryDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x + 1 * cubeScale, pos.y + 1 * cubeScale, pos.z - 1 * cubeScale), AddCubeToList, true, tertiaryDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x + 1 * cubeScale, pos.y + 1 * cubeScale, pos.z + 1 * cubeScale), AddCubeToList, true, tertiaryDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x - 1 * cubeScale, pos.y - 1 * cubeScale, pos.z - 1 * cubeScale), AddCubeToList, true, tertiaryDamage));
            spawnCubeCommandBuffer.Add(new SpawnCubeCommand(cubePrefab, new Vector3(pos.x - 1 * cubeScale, pos.y - 1 * cubeScale, pos.z + 1 * cubeScale), AddCubeToList, true, tertiaryDamage));
        }
        //public void SpawnCube(Cube cube, Vector3 position, Action<Cube> addCubeToLists, bool checkIfPositionAvailable, int overkillDmg) {
        //    if (checkIfPositionAvailable) {
        //        foreach (var item in positionsLog) {
        //            if (position == item) return;
        //        }
        //    }
        //    Cube tmp = Instantiate(cube, position, Quaternion.identity)
        //                //.withID((ulong)((position.x + 1) * (position.z + 1) * (position.y + 1)))
        //                .withSystem(this);
        //    addCubeToLists(tmp);
        //    if(overkillDmg > 0 || !checkIfPositionAvailable) {
        //        tmp.TakeDamage(overkillDmg); //THIS FUCKS THE APP LOL, NEVER DO THIS
        //    }
        //}
        public void SpawnCubes(ref List<SpawnCubeCommand> commandList) {
            while (commandList.Count > 0) {
                SpawnCube(commandList.First());
                commandList.RemoveAt(0);
            }
        }

        public void SpawnCube(SpawnCubeCommand command) {
            if (command.checkIfPositionAvailable) {
                foreach (var item in positionsLog) {
                    if (command.position == item) {
                        //Debug.Log("Position already exists");
                        return;
                    }
                }
            }
            CubeType type = (CubeType)Random.Range(1, 6);
            Debug.Log(type);
            int maxHp = ((int)type) * 3;
            Debug.Log(maxHp);
            if(command.overkillDmg <= maxHp) {
                Cube tmp = Instantiate(command.cube, command.position, Quaternion.identity)
                        //.withID((ulong)((position.x + 1) * (position.z + 1) * (position.y + 1)))
                        .withSystem(this)
                        .withMaxHP(maxHp)
                        .withMaterial(materials[(int)type])
                        .withType(type);
                AddCubeToList(tmp);
                tmp.transform.parent = this.transform;
                if (command.overkillDmg > 0 || !command.checkIfPositionAvailable) {
                    tmp.TakeDamage(command.overkillDmg); //THIS FUCKS THE APP LOL, NEVER DO THIS
                }
            }
            else {
                DestroyCube(null, command.position, command.overkillDmg - 3);
            }
        }

        //public void SpawnCube(Cube cube, Vector3 position, Action<Cube> addCubeToLists, bool checkIfPositionAvailable) {
        //    if (checkIfPositionAvailable) {
        //        foreach (var item in positionsLog) {
        //            if (position == item) return;
        //        }
        //    }
        //    Cube tmp = Instantiate(cube, position, Quaternion.identity)
        //                //.withID((ulong)((position.x + 1) * (position.z + 1) * (position.y + 1)))
        //                .withSystem(this);
        //    tmp.transform.parent = this.transform;
        //    addCubeToLists(tmp);
        //}

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
