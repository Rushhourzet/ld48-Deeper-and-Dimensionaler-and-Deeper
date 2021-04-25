using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static DaD.AddressingSystem;

namespace DaD {
    public class PlayerSystem : MonoBehaviour{
        public Player playerPrefab;
        public AddressingSystem a_System;
        private Player player;

        public void Initialize() {
            player = Instantiate(playerPrefab, new Vector3(15f, 6f, 15f), Quaternion.identity)
                .withMoney(500);
        }

        public void UpdatePlayerMoneyWhenCubeGetsDestroyed(Cube cube) {
            switch (cube.type) {
                case CubeType.dirt:
                    player.money += 10;
                    break;
                case CubeType.silium:
                    player.money += 5;
                    break;
                case CubeType.gravel:
                    player.money += 20;
                    break;
                case CubeType.softWood:
                    player.money += 30;
                    break;
                case CubeType.hardWood:
                    player.money += 100;
                    break;
                case CubeType.rockFoam:
                    player.money += 150;
                    break;
            }
            a_System.PlayerMoneyUpdated.Invoke();
        }
        public void PlayerDamageUpdated() {
            GetPlayerDamage(out int damage);
            a_System.PlayerDamageUpdated(damage, a_System.playerDamageUpdatedAction);
        }
        public void GetPlayerMoney(out ulong money) {
            money = player.money;
        }
        public void GetPlayerDamage(out int damage) {
            damage = player.damage;
        }
    }
}
