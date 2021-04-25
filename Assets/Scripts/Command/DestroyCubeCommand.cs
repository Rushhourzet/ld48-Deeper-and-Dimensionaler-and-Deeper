using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DaD {
    public struct DestroyCubeCommand {
        public Cube cube;
        public Vector3 pos;
        public int overkillDamage;

        public DestroyCubeCommand(Cube cube, Vector3 pos, int overkillDamage) {
            this.cube = cube;
            this.pos = pos;
            this.overkillDamage = overkillDamage;
        }
    }
}