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
        public UiSystem u_system;
        private Player player;
        public ulong initialDamageCost = 100;
        private ulong damageCost;
        private ulong score;

        public void Initialize() {
            score = 0;
            player = Instantiate(playerPrefab, new Vector3(15f, 5f, 15f), Quaternion.identity)
                .withMoney(500)
                .withPlayerSystem(this);
            damageCost = initialDamageCost;
        }
        public void UpdatePlayerMoneyWhenCubeGetsDestroyed(Cube cube) {
            switch (cube.type) {
                case CubeType.dirt:
                    player.money += 20;
                    score += 10;
                    break;
                case CubeType.silium:
                    player.money += 5;
                    score += 5;
                    break;
                case CubeType.gravel:
                    player.money += 30;
                    score += 20;
                    break;
                case CubeType.softWood:
                    player.money += 50;
                    score += 30;
                    break;
                case CubeType.hardWood:
                    player.money += 70;
                    score += 70;
                    break;
                case CubeType.rockFoam:
                    player.money += 100;
                    score += 100;
                    break;
            }
            a_System.PlayerMoneyUpdated.Invoke();
            u_system.UpdateScoreDisplay();
        }

        internal void GetScore(out ulong score) {
            score = this.score;
        }

        public void Buy_IncrementPlayerDamage() {
            GetPlayerDamage(out int damage);
            GetPlayerMoney(out ulong money);
            if (money >= damageCost) {
                player.money -= damageCost;
                u_system.UpdateMoneyDisplay();
                player.damage += 2;
                PlayerDamageUpdated();
                damageCost *= 2;

                u_system.UpdateIncrementDamageCostDisplay(damageCost);
            }

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
