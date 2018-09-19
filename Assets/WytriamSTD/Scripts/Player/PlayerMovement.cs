using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 4.0f;
    public float gravity = 20.0f;
    public float jumpSpeed = 8.0f;

    private CharacterController characterController;
    private bool allowMovement = true;
    private Vector3 moveDirection = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        characterController = this.GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {
        // Handle Movement
        // adapated from https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
        if (allowMovement)
        {
            if (characterController.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;
                if (Input.GetButton("Jump"))
                    moveDirection.y = jumpSpeed;
            }
            moveDirection.y -= gravity * Time.deltaTime;
            characterController.Move(moveDirection * Time.deltaTime);
        }
    }

    public void ToggleMovement()
    {
        allowMovement = !allowMovement;
    }

}
