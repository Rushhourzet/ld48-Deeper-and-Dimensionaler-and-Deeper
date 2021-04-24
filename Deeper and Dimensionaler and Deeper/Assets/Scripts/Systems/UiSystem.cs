using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DaD {
    public class UiSystem : MonoBehaviour {
        public AddressingSystem a_System;
        public PlayerSystem p_System;
        public Text moneyDisplay;

        public void UpdateMoneyDisplay() {
            p_System.GetPlayerMoney(out ulong money);
            moneyDisplay.text = $"{money} $";
        }
    }
}