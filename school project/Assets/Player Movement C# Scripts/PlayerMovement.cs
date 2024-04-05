using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float MoveSpeed;
    public float WalkSpeed;
    public float SprintSpeed;


    public float GroundDrag;

    public float AirDashSpeed;
    public float GroundDashSpeed;
    public float DashSpeedChangeFactror;
    public float GroundDashSpeedChangeFactror;

    public float SlideSpeed;
    public float SlideSpeedChangeFactror;

    private float DesireMoveSpeed;
    private float LastDesireMoveSpeed;


    public float SpeedIncreaseMultiplier;
    public float SlopeIncreaseMultiplier;

    [Header("Jump")]
    public float JumpForce;
    public float JumpCoolDown;
    public float AirMultiplier;
    bool ReadyToJump;


    [Header("DoubleJump")]
    public float DoubleJumpforce;
    bool ReadyToDoubleJump;


    [Header("Crouch")]
    public float CrouchSpeed;
    public float CrouchHeight;
    private float StandingHeight;
    private bool Crouching;

    [Header("KeyBinds")]
    public KeyCode JumpKey = KeyCode.Space;
    public KeyCode SprintKey = KeyCode.LeftShift;
    public KeyCode CrouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float PlayerHeight;
    public LayerMask Ground;
    bool OnGround;

    [Header("Slope Handel")]
    public float MaxSlopeAngel;
    private RaycastHit SlopeHit;
    private bool ExitingSlope;

    public Transform Oreientation;

    float Horizontal;
    float Vertical;

    Vector3 MoveDirection;

    Rigidbody rb;

    public MovementState State;
    public enum MovementState
    {
        Walking, Sprinting, Air, Crouching, Sliding, Dashing
    }

    public bool sliding;
    public bool Dashing;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ReadyToJump = true;
        StandingHeight = transform.localScale.y;
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void Update()
    {
        OnGround = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.3f);

        MyInput();
        SpeedControl();
        StateHandle();

        if (State == MovementState.Walking || State == MovementState.Sprinting || State == MovementState.Crouching)
            rb.drag = GroundDrag;
        else
            rb.drag = 0;

        if (Input.GetKeyDown(CrouchKey) && OnGround)
        {
            transform.localScale = new Vector3(transform.localScale.x, CrouchHeight, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Force);
            State = MovementState.Crouching;
        }
        else if (Input.GetKey(CrouchKey) && !OnGround)
        {
            transform.localScale = new Vector3(transform.localScale.x, CrouchHeight, transform.localScale.z);
        }
        if (Input.GetKeyDown(CrouchKey) && State == MovementState.Crouching)
        {
            transform.localScale = new Vector3(transform.localScale.x, StandingHeight, transform.localScale.z);
            State = MovementState.Walking;
        }
    }

    private void MyInput()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        // Jump
        if (Input.GetKey(JumpKey) && ReadyToJump && OnGround)
        {
            ReadyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), JumpCoolDown);
        }

    }

    private MovementState  LastState;
    private bool KeepMomentum;

    private void StateHandle()
    {

        if (Dashing && !OnGround)
        {
            State = MovementState.Dashing;
            DesireMoveSpeed = AirDashSpeed;
            SpeedIncreaseMultiplier = DashSpeedChangeFactror;
        }
        else if(Dashing && OnGround)
        {
            State = MovementState.Dashing;
            DesireMoveSpeed = GroundDashSpeed;
            SpeedIncreaseMultiplier = GroundDashSpeedChangeFactror;
        }

         else if(sliding)
        {
            State = MovementState.Sliding;

            if (OnSlope() && rb.velocity.y < 0.1)
                DesireMoveSpeed = SlideSpeed;
            else
                DesireMoveSpeed = SprintSpeed;
            SpeedIncreaseMultiplier = SlideSpeedChangeFactror;

        }
        // Crouching Mode
        else if (Input.GetKeyDown(CrouchKey) && OnGround)
        {
            State = MovementState.Crouching;
            DesireMoveSpeed = CrouchSpeed;
        }

        // Sprinting Mode
        else if (OnGround && Input.GetKey(SprintKey))
        {
            State = MovementState.Sprinting;
            DesireMoveSpeed = SprintSpeed;
        }

        // Walking Mode
        else if (OnGround)
        {
            State = MovementState.Walking;
            DesireMoveSpeed = WalkSpeed;
        }

        //  in Air
        else
        {
            State = MovementState.Air;

            MoveSpeed = DesireMoveSpeed;

            if (DesireMoveSpeed < SprintSpeed)
            {
                DesireMoveSpeed = WalkSpeed;
            }
            else
            {
                DesireMoveSpeed = SprintSpeed;
            }

            // double jump
            if (Input.GetKeyDown(JumpKey) && ReadyToDoubleJump)
            {
                DoubleJump();
                ReadyToDoubleJump = false;  
            }
                
        }
        bool DesireMoveSpeedHasChanged = DesireMoveSpeed != LastDesireMoveSpeed;

        if (Mathf.Abs(DesireMoveSpeed - LastDesireMoveSpeed) > (SprintSpeed - WalkSpeed) && MoveSpeed != 0 )
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else if (DesireMoveSpeedHasChanged && KeepMomentum)
        {
                StopAllCoroutines();
                SmoothlyLerpMoveSpeed();
        }
        else
        {

            MoveSpeed = DesireMoveSpeed;
        }

        if ((LastState == MovementState.Dashing)) KeepMomentum = true;

        LastDesireMoveSpeed = DesireMoveSpeed;
        LastState = State;  
    }



    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float Diffrence = Mathf.Abs(DesireMoveSpeed - MoveSpeed);
        float StartValue = MoveSpeed;


        while (time < Diffrence)
        {
            MoveSpeed = Mathf.Lerp(StartValue, DesireMoveSpeed, time / Diffrence);

            if (OnSlope())
            {
                float SLopeAngel = Vector3.Angle(Vector3.up, SlopeHit.normal);
                float SLopeAngelIncrease = 1 + (SLopeAngel / 90f);

                time += Time.deltaTime * SpeedIncreaseMultiplier * SlopeIncreaseMultiplier * SLopeAngelIncrease;
            }
            else
                time += Time.deltaTime * SpeedIncreaseMultiplier;

            yield return null;

        }

        MoveSpeed = DesireMoveSpeed;
        KeepMomentum = false;
    }


    private void PlayerMove()
    {
        if (State == MovementState.Dashing) return;


        MoveDirection = Oreientation.forward * Vertical + Oreientation.right * Horizontal;

        // On slope
        if (OnSlope() && !ExitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(MoveDirection) * MoveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y < 0.1)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        // turn off gravity while on slope
        rb.useGravity = !OnSlope();

        // On ground
        if (OnGround)
            rb.AddForce(MoveDirection.normalized * MoveSpeed * 10f, ForceMode.Force);

        // In air
        else if (!OnGround)
            rb.AddForce(MoveDirection.normalized * MoveSpeed * 10f * AirMultiplier, ForceMode.Force);

    }

    private void SpeedControl()
    {
        // limit speed on slope
        if (OnSlope() && !ExitingSlope)
        {
            if(rb.velocity.magnitude > MoveSpeed)
            {
                rb.velocity = rb.velocity.normalized * MoveSpeed;
            }
        }
        else
        {
            Vector3 FlatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (FlatVelocity.magnitude > MoveSpeed)
            {
                Vector3 Limitedvelocity = FlatVelocity.normalized * MoveSpeed;
                rb.velocity = new Vector3(Limitedvelocity.x, rb.velocity.y, Limitedvelocity.z);
            }
        }
    }
    private void Jump()
    {
        ExitingSlope = true;   

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    }

    private void DoubleJump()
    {
        ExitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * DoubleJumpforce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        ReadyToJump = true;

        ReadyToDoubleJump = true;

        ExitingSlope = false;   
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out SlopeHit, PlayerHeight * 0.5f + 0.3f))
        {
            float angel = Vector3.Angle(Vector3.up, SlopeHit.normal);
            return angel < MaxSlopeAngel && angel != 0;
        }
        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 Direction)
    {
        return Vector3.ProjectOnPlane(Direction, SlopeHit.normal).normalized;
    }

}
    

