using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DaD {
    public class PlayerSytem : MonoBehaviour{
        public Player playerPrefab;
        public AddressingSystem a_System;

        public void Initialize() {
            Instantiate(playerPrefab, new Vector3(15f, 2f, 15f), Quaternion.identity);
        }
    }
}
