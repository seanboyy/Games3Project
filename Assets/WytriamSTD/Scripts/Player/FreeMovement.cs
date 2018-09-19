using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMovement : MonoBehaviour
{ 
    public float speed = 4.0f;
    public float sprintMultiplier = 2.0f;

    private CharacterController characterController;
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
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (Time.timeScale == 0)
            moveDirection = Vector3.zero;
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        if (Input.GetKey(KeyCode.E))
            moveDirection.y = speed;
        if (Input.GetKey(KeyCode.Q))
            moveDirection.y = -speed;
        if (Input.GetKey(KeyCode.LeftShift))
            moveDirection *= sprintMultiplier;

        characterController.Move(moveDirection * Time.unscaledDeltaTime);
    }
}
