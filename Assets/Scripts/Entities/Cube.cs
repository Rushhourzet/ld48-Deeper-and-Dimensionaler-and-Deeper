using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static DaD.AddressingSystem;

namespace DaD {
    public class Cube : MonoBehaviour {
        public CubeSystem system;
        public CubeType type;
        public bool isAlive;
        public bool isActive;
        public int maxHp;
        public ulong id;
        private int hp;
        private float delta;

        private Renderer renderer => GetComponent<Renderer>();
        private CubeDestroyedAction cubeDestroyedAction_destroy;

        public Cube withID(in ulong id) {
            this.id = id;
            return this;
        }
        public Cube withSystem(in CubeSystem system) {
            this.system = system;
            return this;
        }
        public Cube withMaxHP(in int maxHp) {
            this.maxHp = maxHp;
            this.hp = maxHp;
            return this;
        }
        public Cube withType(in CubeType type) {
            this.type = type;
            return this;
        }
        public Cube withMaterial(in Material mat) {
            renderer.material = mat;
            return this;
        }

        //public Cube IsActive(in bool isActive) {
        //    this.isActive = isActive;
        //    return this;
        //}
        //public Cube IsAlive(in bool isAlive) {
        //    this.isAlive = isAlive;
        //    return this;
        //}
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
        private void OnMouseOver() {
            delta += Time.deltaTime;
            if(delta >= .5f) {
                if (Cursor.lockState == CursorLockMode.Locked)
                    TakeDamage(system.playerDamage);
                delta = 0;
            }
        }
        private void OnMouseDown() {
            if (Cursor.lockState == CursorLockMode.Locked)
                TakeDamage(system.playerDamage);
        }
        public void TakeDamage(int damage) {
            hp -= damage;
            if (hp <= 0) {
                int overkillDmg = hp * -1;
                //Debug.Log(overkillDmg);
                if(this != null) {
                    system.AddDestroyCubeCommandToBuffer(new DestroyCubeCommand(this, this.transform.position, overkillDmg));
                }
            }
            //Debug.Log("You clicked me, my HP are " + hp);
        }
    }
    public enum CubeType {
        silium = 1,
        dirt,
        gravel,
        softWood,
        hardWood,
        rockFoam
    }
}
