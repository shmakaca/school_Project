using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float MoveSpeed;
    public float WalkSpeed;
    public float SprintSpeed;
    private float DesireMoveSpeed;
    private float LastDesireMoveSpeed;
    public float DashSpeed;
    public float SlideSpeed;
    public float WallRunSpeed;
    public float MaxYSpeed;
    public float KnockBackSpeed;

    [Header("keep momentum")]
    private float SpeedIncreaseMultiplier;
    public float DashSpeedIncreaseMultiplier;
    public float SlopeIncreaseMultiplier;
    public float SlideSpeedChangeFactror;
    public float KnockBackSpeedChangeFactor;
    bool MaxSpeed;

    [Header("Jump")]
    public float JumpForce;
    public float JumpCoolDown;
    public float AirMultiplier;
    public float JumpPadCheckDistance;
    public bool ReadyToJump;


    [Header("DoubleJump")]
    public float DoubleJumpforce;
    public bool ReadyToDoubleJump;


    [Header("Crouch")]
    public float CrouchSpeed;
    public float CrouchHeight;
    private float StandingHeight;
    private bool Crouching;

    [Header("Ground Check")]
    public float PlayerHeight;
    public LayerMask Ground;
    public float GroundDrag;
    public bool OnGround;

    [Header("JumpPads Check")]
    public LayerMask JumpPad;
    public float JumpPadforce;
    public bool OnJumpPad;



    [Header("Slope Handel")]
    public float MaxSlopeAngel;
    private RaycastHit SlopeHit;
    private bool ExitingSlope;

    [Header("Refrences")]
    public Transform Oreientation;
    public GameObject PlayerObject;
    public GameObject CameraHolder;
    public GameObject KeyBindMenu;
    public playercamera Cam;
    public Camera PlayerCamera;
    private WallRun WallRun;
    private KeybindManager KeybindManager;

    [Header("Fov")]
    public float NormalFov;
    public float JumPadFovChange;
    public float DashFovChange;
    public float WallRunFovChange;
    public float ShootingFovChange;
    public float SprintFovChange;
    public float SlideFovChange;
    public Slider FOVslider;

    public float Horizontal;
    public float Vertical;

    Vector3 MoveDirection;

    Rigidbody rb;

    public MovementState State;
    public enum MovementState
    {
        Walking, Sprinting, Air, Crouching, Sliding, Dashing, WallRunning, shooting, Standing
    }

    public bool sliding;
    public bool Dashing;
    public bool WallRunning;
    public bool Shooting;
    bool isCrouching = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ReadyToJump = true;
        StandingHeight = transform.localScale.y;
        PlayerCamera = CameraHolder.GetComponent<Camera>();
        KeybindManager = KeyBindMenu.GetComponent<KeybindManager>();
        WallRun = GetComponent<WallRun>();

        FOVslider.value = FOVslider.minValue;
    }

    private void FixedUpdate()
    {
        PlayerMove();
        NormalFov = FOVslider.value;
    }

    public void Update()
    {



        OnGround = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.3f);
        OnJumpPad = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.3f, JumpPad);

        MyInput();
        SpeedControl();
        StateHandle();

        if (State == MovementState.Walking || State == MovementState.Sprinting || State == MovementState.Crouching)
            rb.drag = GroundDrag;
        else
            rb.drag = 0;

        if (Input.GetKeyDown(KeybindManager.GetKeyCode("Crouch")))
        {
            isCrouching = !isCrouching; 
        }

        if (isCrouching)
        {
            transform.localScale = new Vector3(transform.localScale.x, CrouchHeight, transform.localScale.z);
            if (OnGround)
            {
                rb.AddForce(Vector3.down * 5f, ForceMode.Force);
            }
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, StandingHeight, transform.localScale.z);
        }

        HandleJumpPad();


    }


    private void MyInput()
    {
        Horizontal = 0;
        Vertical = 0;

        // Forward and backward
        if (Input.GetKey(KeybindManager.GetKeyCode("Forward")))
        {
            Vertical += 1;
        }
        if (Input.GetKey(KeybindManager.GetKeyCode("BackWards")))
        {
            Vertical -= 1;
        }

        // Left and right
        if (Input.GetKey(KeybindManager.GetKeyCode("Left")))
        {
            Horizontal -= 1;
        }
        if (Input.GetKey(KeybindManager.GetKeyCode("Right")))
        {
            Horizontal += 1;
        }

        // Jump
        if (Input.GetKey(KeybindManager.GetKeyCode("Jump")) && ReadyToJump && OnGround)
        {
            ReadyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), JumpCoolDown);
        }

    }

    private MovementState LastState;

    private void StateHandle()
    {
        if (Shooting)
        {
            State = MovementState.shooting;
            DesireMoveSpeed = KnockBackSpeed;
            SpeedIncreaseMultiplier = KnockBackSpeedChangeFactor;
            Cam.DOFOV(ShootingFovChange + NormalFov);
            Cam.DOTilt(0f);
        }


        else if (WallRunning)
        {
            State = MovementState.WallRunning;
            DesireMoveSpeed = WallRunSpeed;
            Cam.DOFOV(WallRunFovChange + NormalFov);
            if (WallRun.LeftWall)
                Cam.DOTilt(5f);
            if (!WallRun.LeftWall)
                Cam.DOTilt(-5f);
        }

        else if (Dashing)
        {
            State = MovementState.Dashing;
            DesireMoveSpeed = DashSpeed;
            SpeedIncreaseMultiplier = DashSpeedIncreaseMultiplier;
            Cam.DOFOV(DashFovChange + NormalFov);
            Cam.DOTilt(0f);
        }

        else if (sliding)
        {
            State = MovementState.Sliding;

            SlideFovChange += Time.deltaTime * 4f;

            if (OnSlope() && rb.velocity.y < 0.1)
            {
                Cam.DOFOV(SlideFovChange + NormalFov);
                DesireMoveSpeed = SlideSpeed;
            }
            else
            {
                DesireMoveSpeed = SprintSpeed;
                Cam.DOFOV(NormalFov);
                SlideFovChange = 5f;
            }
            SpeedIncreaseMultiplier = SlideSpeedChangeFactror;

        }
        // Crouching Mode
        else if (isCrouching && OnGround)
        {
            State = MovementState.Crouching;
            DesireMoveSpeed = CrouchSpeed;
            Cam.DOFOV(NormalFov);
            Cam.DOTilt(0f);
        }

        // Sprinting Mode
        else if (OnGround && Input.GetKey(KeybindManager.GetKeyCode("Sprint")))
        {
            State = MovementState.Sprinting;
            DesireMoveSpeed = SprintSpeed;
            Cam.DOFOV(NormalFov + SprintFovChange);
            Cam.DOTilt(0f);
            if (Dashing)
            {
                State = MovementState.Dashing;
                DesireMoveSpeed = DashSpeed;
                SpeedIncreaseMultiplier = DashSpeedIncreaseMultiplier;
                Cam.DOFOV(DashFovChange + NormalFov);
                Cam.DOTilt(0f);
            }
                  
        }
        // Walking Mode
        else if (OnGround)
        {
            State = MovementState.Walking;
            DesireMoveSpeed = WalkSpeed;
            Cam.DOFOV(NormalFov);
            Cam.DOTilt(0f);
            if (Dashing)
            {
                State = MovementState.Dashing;
                DesireMoveSpeed = DashSpeed;
                SpeedIncreaseMultiplier = DashSpeedIncreaseMultiplier;
                Cam.DOFOV(DashFovChange + NormalFov);
                Cam.DOTilt(0f);
            }
        }

        //  in Air
        else
        {
            State = MovementState.Air;

            
            Cam.DOFOV(NormalFov);
            Cam.DOTilt(0f);

            if (DesireMoveSpeed < SprintSpeed)
            {
                DesireMoveSpeed = WalkSpeed;
            }
            else if (Dashing)
            {
                State = MovementState.Dashing;
                DesireMoveSpeed = DashSpeed;
                SpeedIncreaseMultiplier = DashSpeedIncreaseMultiplier;
                Cam.DOFOV(DashFovChange + NormalFov);
                Cam.DOTilt(0f);
            }
            else
            {
                DesireMoveSpeed = SprintSpeed;
            }

            // double jump
            if (Input.GetKeyDown(KeybindManager.GetKeyCode("Jump")) && ReadyToDoubleJump)
            {
                DoubleJump();
                ReadyToDoubleJump = false;
            }

        }


        if (Mathf.Abs(DesireMoveSpeed - LastDesireMoveSpeed) > (SprintSpeed - CrouchSpeed) && MoveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            MoveSpeed = DesireMoveSpeed;
        }

        LastDesireMoveSpeed = DesireMoveSpeed;

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

                time += Time.deltaTime * SpeedIncreaseMultiplier * SLopeAngelIncrease;
            }
            else
                time += Time.deltaTime * SpeedIncreaseMultiplier;

            yield return null;

        }

        MoveSpeed = DesireMoveSpeed;
    }


    private void PlayerMove()
    {

        MoveDirection = Oreientation.forward * Vertical + Oreientation.right * Horizontal;

        // On slope
        if (OnSlope() && !ExitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(MoveDirection) * MoveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y < 0)
            {
                rb.AddForce(Vector3.down * 100f, ForceMode.Force);
            }
        }

        // On ground
        else if (OnGround)
            rb.AddForce(MoveDirection.normalized * MoveSpeed * 10f, ForceMode.Force);

        // In air
        else if (!OnGround)
            rb.AddForce(MoveDirection.normalized * MoveSpeed * 10f * AirMultiplier, ForceMode.Force);

        // turn off gravity while on slope
        if (!WallRunning)
        {
            rb.useGravity = !OnSlope();
        }

    }

    private void SpeedControl()
    {
        // limit speed on slope
        if (OnSlope() && !ExitingSlope)
        {
            if (rb.velocity.magnitude > MoveSpeed)
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

        if (MaxYSpeed != 0 && rb.velocity.y > MaxYSpeed)
            rb.velocity = new Vector3(rb.velocity.x, MaxYSpeed, rb.velocity.z);
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

    private void HandleJumpPad()
    {
        if (OnJumpPad)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // reset y velocity
            rb.AddForce(Vector3.up * JumpPadforce, ForceMode.Impulse);

            Cam.DOFOV(JumPadFovChange + NormalFov);

            // limit max y speed
            if (rb.velocity.y > MaxYSpeed)
            {
                rb.velocity = new Vector3(rb.velocity.x, MaxYSpeed, rb.velocity.z);
            }
        }
    }


}


