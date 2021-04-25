using DaD;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
namespace DaD {
    public class Player : MonoBehaviour {
        public delegate void MovePlayerDel(in PlayerInputDel input);
        public delegate void RotatePlayerDel(in PlayerInputDel rotation);
        public delegate void PlayerInputDel(out Vector3 vector);
        private MovePlayerDel moveplayer => MovePlayer;
        private RotatePlayerDel rotateplayer => RotatePlayer;
        private PlayerInputDel gatherMoveInput => GatherMovementInput;
        private PlayerInputDel gatherRotInput => GatherRotationInput;

        public Camera playerCamera;
        private Rigidbody rb => GetComponent<Rigidbody>();
        //private Collider coll => GetComponent<CapsuleCollider>();
        public float sens = 5f;
        public float ms = 5f;

        public ulong money;
        public int damage = 1;
        private void FixedUpdate() {
        }

        void Update() {
            if(Cursor.lockState == CursorLockMode.Locked) {
                PlayerMovement(moveplayer, rotateplayer);
            }
            CursorLockHandle();
        }
        public Player withMoney(ulong money) {
            this.money = money;
            return this;
        }
        public void PlayerMovement(in MovePlayerDel movePlayer, in RotatePlayerDel rotatePlayer) {
            movePlayer(gatherMoveInput);
            rotatePlayer(gatherRotInput);
        }

        public void GatherMovementInput(out Vector3 movement) {
            movement = new Vector3(
                Input.GetAxis("Horizontal"),
                Input.GetButton("Jump") == true ? 1f : 0f,
                Input.GetAxis("Vertical"));
        }
        public void GatherRotationInput(out Vector3 rotation) {
            rotation = new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        }
        private void MovePlayer(in PlayerInputDel input) {
            input(out Vector3 movement);
            transform.Translate(movement * Time.deltaTime * ms);
        }
        private void RotatePlayer(in PlayerInputDel input) {
            input(out Vector3 rotation);
            transform.Rotate(0f, rotation.y * sens, 0f);
            playerCamera.transform.Rotate(rotation.x * sens * -1, 0f, 0f);
        }
        private void CursorLockHandle() {
            if (Cursor.lockState == CursorLockMode.None && Input.GetButton("Fire1")) {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if (Cursor.lockState == CursorLockMode.Locked && Input.GetButtonDown("Cancel")) {
                Cursor.lockState = CursorLockMode.None;
            }
        }

    }
}