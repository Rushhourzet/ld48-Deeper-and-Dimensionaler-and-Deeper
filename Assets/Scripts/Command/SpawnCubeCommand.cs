using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DaD {
    public struct SpawnCubeCommand{
        public Cube cube;
        public Vector3 position;
        public Action<Cube> addCubeToLists;
        public bool checkIfPositionAvailable;
        public int overkillDmg;

        public SpawnCubeCommand(Cube cube, Vector3 position, Action<Cube> addCubeToLists, bool checkIfPositionAvailable, int overkillDmg) : this() {
            this.cube = cube;
            this.position = position;
            this.addCubeToLists = addCubeToLists;
            this.checkIfPositionAvailable = checkIfPositionAvailable;
            this.overkillDmg = overkillDmg;
        }
    }
}
