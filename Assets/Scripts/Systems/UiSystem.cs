using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DaD {
    public class UiSystem : MonoBehaviour {
        public AddressingSystem a_System;
        public PlayerSystem p_System;
        public GameObject shopPanel;
        public GameObject pausePanel;
        public Text scoreText;
        public Text moneyDisplay;
        public Text incrementDamageCostDisplay;
        public Text timeText;
        public Text pressEscHintText;
        private bool hintDisabled;
        public Button incrementDamageButton;
        private void Update() {
            int timeSinceStart = (int)Time.realtimeSinceStartup;
            int seconds = timeSinceStart%60;
            int minutes = (timeSinceStart/60)%60;
            int hours = (minutes / 60);
            if(timeSinceStart < 60) {
                timeText.text = $"{seconds}";
            } 
            else if(timeSinceStart >= 60) {
                timeText.text = $"{minutes} : {seconds}";
                if (!hintDisabled) {
                    pressEscHintText.gameObject.SetActive(false);
                    hintDisabled = true;
                }
            }
            else if(timeSinceStart >= 60*60) {
                timeText.text = $"{hours} : {minutes} : {seconds}";
            }

            if (Input.GetButtonDown("Shop")) {
                shopPanel.SetActive(!shopPanel.activeSelf);
            }
            if (Input.GetButtonDown("Cancel")) {
                pausePanel.SetActive(!pausePanel.activeSelf);
            }
            if(Cursor.lockState == CursorLockMode.Locked && (shopPanel.activeSelf || pausePanel.activeSelf)) {
                Cursor.lockState = CursorLockMode.None;
            }      
            if(Cursor.lockState == CursorLockMode.None && !shopPanel.activeSelf && !pausePanel.activeSelf) {
                Cursor.lockState = CursorLockMode.Locked;
            }            
        }
        public void UpdateMoneyDisplay() {
            p_System.GetPlayerMoney(out ulong money);
            moneyDisplay.text = $"{money} $";
        }
        public void UpdateScoreDisplay() {
            p_System.GetScore(out ulong score);
            scoreText.text = $"{score}";
        }
        public void UpdateIncrementDamageCostDisplay(ulong damageCost) {
            incrementDamageCostDisplay.text = $"{damageCost} $";
        }
        public void ContinueButtonPressed() {
            pausePanel.SetActive(false);
        }
    }
}