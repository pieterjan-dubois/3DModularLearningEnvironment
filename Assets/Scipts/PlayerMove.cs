using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float MoveSmoothTime;
    public float GravityStrength;
    public float JumpStrength;
    public float WalkSpeed;
    public float RunSpeed;

    private CharacterController controller;
    private Vector3 CurrentMoveVelocity;
    private Vector3 MoveDampVelocity;
    private Vector3 CurrentForceVelocity;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 PlayerInput = new Vector3
        {
            x = Input.GetAxis("Horizontal"),
            y = 0f,
            z = Input.GetAxis("Vertical")
        };

        if(PlayerInput.magnitude > 1f)
        {
            PlayerInput.Normalize();
        }

        Vector3 MoveVector = transform.TransformDirection(PlayerInput);
        float CurrentSpeed = Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed;

        CurrentMoveVelocity = Vector3.SmoothDamp(
            CurrentMoveVelocity, 
            MoveVector * CurrentSpeed, 
            ref MoveDampVelocity, 
            MoveSmoothTime
        );

        controller.Move(CurrentMoveVelocity * Time.deltaTime);
        /*if(Physics.Raycast(groundCheckRay, 1.1f))
        {
            CurrentForceVelocity.y = -2f;

            if(Input.GetKey(KeyCode.Space))
            {
                CurrentForceVelocity.y = JumpStrength;
            }
            else
            {
                CurrentForceVelocity.y -= GravityStrength * Time.deltaTime;
            }

            controller.Move(CurrentForceVelocity * Time.deltaTime);
        }*/
    }
}
