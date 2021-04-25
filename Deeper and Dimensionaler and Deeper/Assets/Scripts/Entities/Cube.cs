using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DaD.AddressingSystem;

namespace DaD {
    public class Cube : MonoBehaviour {
        public CubeSystem system;
        public CubeType type;
        public int maxHp;
        public ulong id;
        private int hp;

        private CubeDestroyedAction cubeDestroyedAction_destroy;

        public Cube withID(in ulong id) {
            this.id = id;
            return this;
        }
        public Cube withSystem(in CubeSystem system) {
            this.system = system;
            return this;
        }
        //public Cube withType(in CubeType type) {
        //    this.type = type;
        //    this.GetComponent<Renderer>().material.SetTexture()
        //    return this;
        //}
        //public static void GetValuesByType(in CubeType type, out Texture texture, out int bounty) {
        //    switch(type){
        //        case CubeType.dirt:
        //            texture = textures[0];
        //            bounty = 5;
        //            break;
        //        case CubeType.silium:
        //            texture = textures[1];
        //            bounty = 10;
        //            break;
        //        }
        //}
        private void Start() {
            maxHp = 3;
            hp = maxHp;
        }
        private void OnMouseDown() {
            TakeDamage(system.playerDamage);
        }
        public void TakeDamage(int damage) {
            hp-= damage;
            if (hp <= 0) {
                int overkillDmg = hp * -1;
                system.DestroyCube(this, overkillDmg);
            }
            Debug.Log("You clicked me, my HP are " + hp);
        }
    }
    public enum CubeType {
        silium,
        dirt,
        gravel,
        softWood,
        hardWood,
        rockFoam
    }
}
