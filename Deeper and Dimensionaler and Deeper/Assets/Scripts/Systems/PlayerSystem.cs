using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DaD {
    public class PlayerSystem : MonoBehaviour{
        public Player playerPrefab;
        public AddressingSystem a_System;
        private Player player;

        public void Initialize() {
            player = Instantiate(playerPrefab, new Vector3(15f, 2f, 15f), Quaternion.identity);
            a_System.cubeDestroyedAction = UpdatePlayerMoneyWhenCubeGetsDestroyed;
        }

        public void UpdatePlayerMoneyWhenCubeGetsDestroyed(Cube cube) {
            if (cube.type == CubeType.dirt) {
                player.money += 5;
            }
            a_System.PlayerMoneyUpdated.Invoke();
        }
        public void GetPlayerMoney(out ulong money) {
            money = player.money;
        }
    }
}
