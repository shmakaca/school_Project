using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Palmmedia.ReportGenerator.Core;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController Controller;

    private void Start()
    {
        Controller = GetComponent<CharacterController>();
        standingHighet = transform.localScale.y + 0.5f;
        crouchingHighet = transform.localScale.y - 1f;
        
    }

    Vector3 moveDirection;// player z and x direction
    Vector3 velocity;// player y direction


    [Header("gravity")]
    public float gravity = -9.81f;// gravity(should be negative)

    [Header("Jumping")]
    public float jumpHeight = 3f;// jump  highet

    [Header("Crouching")]
    private float crouchSpeed = walkSpeed / 3.5f;//crouching speed
    private float crouchingHighet;//your highet when crouch

    [Header("Walking")]
    private float standingHighet;
    public static float walkSpeed = 9f;// the deafult walking speed
    private float SideWaySpeed = walkSpeed * 0.8f;// when you walk sideways (A ans D)
    private float moveSpeed;// moveSpeed is 

    [Header("Sprinting")]
    private float sprintSpeed = walkSpeed * 1.4f;  

    [Header("Dashing")]
    public float dashSpeed;
    public float dashForce;


    [Header("in air")]
    private float airSpeed = walkSpeed / 2;
    private bool ReadyToDoubleJump = true;
    private bool ReadyToDash = true;

    [Header("IsGround")]
    private bool isGround;
    private float groundDistance = 0.4f;
    public Transform groundcheck;
    public LayerMask groundMask;

    private void PlayerMove()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float Vertical = Input.GetAxisRaw("Vertical");
        moveDirection = transform.right * horizontal + transform.forward * Vertical;

        Controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
 
    private void Jumping()
    {
        if (Input.GetButtonDown("Jump") && isGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        }
    }

    private void Dash()
    {
        float horizontal = Input.GetAxisRaw("Horizontal") * dashForce;
        float Vertical = Input.GetAxisRaw("Vertical") * dashForce;
        moveDirection = transform.right * horizontal + transform.forward * Vertical;

        Controller.Move(moveDirection * dashSpeed * Time.deltaTime);
        ReadyToDash = false;

    }
    private void Crouching()
    {
        if(state == MovementState.crouching)
        {
            float horizontal = Input.GetAxisRaw("Horizontal") *crouchSpeed;
            float Vertical = Input.GetAxisRaw("Vertical") * crouchSpeed;
            moveDirection = transform.right * horizontal + transform.forward * Vertical;

            Controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            Controller.height = crouchingHighet;
        }
        else
        {
            Controller.height = standingHighet;
        }
    }

    private void Doublejumping()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -1.5f * gravity);
        ReadyToDoubleJump = false;
    }

    private void Sprinting()
    {
        if ((state == MovementState.sprinting))
        {
            moveSpeed = sprintSpeed;
        }
        
    }
    private void Walking()
    {
        if ((state == MovementState.walking))
        {
            moveSpeed = walkSpeed;
            ReadyToDoubleJump = true;
            ReadyToDash = true;
        }
    }

    private void WalkingSideWays()
    {
        if ((state == MovementState.walkingSideWay))
        {
            moveSpeed = SideWaySpeed;
        }
    }

    private void InAir()
    {
        if ((state == MovementState.air))
        {
            if (Input.GetKeyDown(KeyCode.Space) && ReadyToDoubleJump)
            {
                Doublejumping();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && ReadyToDash)
            {
                Dash();
            }
            moveSpeed = airSpeed;

        }
    }
    private void GravityForce()
    {
        velocity.y += gravity * Time.deltaTime;

        Controller.Move(velocity * Time.deltaTime);

        if (isGround && velocity.y < 0)
        {
            velocity.y = -5f;
        }
    }


    public MovementState state;
    public enum MovementState
    {
        walkingSideWay, walking, sprinting, air, crouching
    }
    private void stateHandel()
    {
        //sprintnig state
        if (isGround && Input.GetKey(KeyCode.LeftShift))
        {
            state = MovementState.sprinting;
            // we dont want sprinting while walkind sideways
            if (isGround && Input.GetKey(KeyCode.A) || isGround && Input.GetKey(KeyCode.D))
            {
                state = MovementState.walkingSideWay;
            }
        }
        //crouching state
        else if (isGround && Input.GetKey(KeyCode.LeftControl))
        {
            state = MovementState.crouching;
        }
        //walking satate
        else if (isGround)
        {
            state = MovementState.walking;
            // we dont want walking sppedas same as sideways speed
            if (isGround && Input.GetKey(KeyCode.A) || isGround && Input.GetKey(KeyCode.D))
            {
                state = MovementState.walkingSideWay;
            }
        }
        // air state
        else
        {
            state = MovementState.air;
        }
    }

    private void Update()
    {
        isGround = Physics.CheckSphere(groundcheck.position, groundDistance, groundMask);

        stateHandel();
            
        PlayerMove(); 
        
        Sprinting();

        Walking();

        InAir();

        WalkingSideWays();

        Jumping();
        
        Crouching();

        GravityForce();

        
    }
}
