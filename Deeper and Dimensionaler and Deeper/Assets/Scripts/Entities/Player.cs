using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void MovePlayerDel(in PlayerInputDel input);
    public delegate void RotatePlayerDel(in PlayerInputDel rotation);
    public delegate void PlayerInputDel(out Vector3 vector);
    private MovePlayerDel moveplayer => MovePlayer;
    private RotatePlayerDel rotateplayer => RotatePlayer;
    private PlayerInputDel gatherMoveInput => GatherMovementInput;
    private PlayerInputDel gatherRotInput => GatherRotationInput;

    public Camera playerCamera;
    public float sens = 0.2f;
    public float ms = 5f;

    void Update()
    {
        PlayerMovement(moveplayer, rotateplayer);
    }

    public void PlayerMovement(in MovePlayerDel movePlayer, in RotatePlayerDel rotatePlayer) {
        movePlayer(gatherMoveInput);
        rotatePlayer(gatherRotInput);
    }

    public void GatherMovementInput( out Vector3 movement) {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
    }
    public void GatherRotationInput( out Vector3 rotation) {
        rotation = new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
    }
    private void MovePlayer(in PlayerInputDel input) {
        input(out Vector3 movement);
        transform.Translate(movement * Time.deltaTime * ms);
    }
    private void RotatePlayer(in PlayerInputDel input) {
        input(out Vector3 rotation);
        transform.Rotate(0f, rotation.y * sens, 0f);
        playerCamera.transform.Rotate(rotation.x * sens *-1, 0f, 0f);
    }
}
