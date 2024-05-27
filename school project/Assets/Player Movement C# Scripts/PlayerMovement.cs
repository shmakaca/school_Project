using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float desireMoveSpeed;
    [SerializeField] private float lastDesireMoveSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float slideSpeed;
    [SerializeField] private float wallRunSpeed;
    [SerializeField] private float maxYSpeed;
    [SerializeField] private float knockBackSpeed;

    [Header("keep momentum")]
    private float speedIncreaseMultiplier;
    [SerializeField] private float dashSpeedIncreaseMultiplier;
    [SerializeField] private float slopeIncreaseMultiplier;
    [SerializeField] private float slideSpeedChangeFactor;
    [SerializeField] private float knockBackSpeedChangeFactor;
    [SerializeField] private bool maxSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCoolDown;
    [SerializeField] private float airMultiplier;
    [SerializeField] private float jumpPadCheckDistance;
    [SerializeField] private bool readyToJump;

    [Header("DoubleJump")]
    [SerializeField] private float doubleJumpForce;
    [SerializeField] private bool readyToDoubleJump;

    [Header("Crouch")]
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchHeight;
    [SerializeField] private float standingHeight;
    [SerializeField] private bool crouching;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float groundDrag;
    [SerializeField]  private bool onGround;

    [Header("JumpPads Check")]
    [SerializeField] private LayerMask jumpPad;
    [SerializeField] private float jumpPadForce;
    [SerializeField] private bool onJumpPad;

    [Header("Slope Handle")]
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private RaycastHit slopeHit;
    [SerializeField] private bool exitingSlope;

    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject cameraHolder;
    [SerializeField] private GameObject keyBindMenu;
    [SerializeField] private playercamera cam;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private WallRun wallRun;
    [SerializeField] private KeybindManager keybindManager;

    [Header("Fov")]
    [SerializeField] private float normalFov;
    [SerializeField] private float jumpPadFovChange;
    [SerializeField] private float dashFovChange;
    [SerializeField] private float wallRunFovChange;
    [SerializeField] private float shootingFovChange;
    [SerializeField] private float sprintFovChange;
    [SerializeField] private float slideFovChange;
    [SerializeField] private Slider fovSlider;

    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;

    [SerializeField] private Vector3 moveDirection;

    [SerializeField] private Rigidbody rb;

    public enum MovementState
    {
        Walking, Sprinting, Air, Crouching, Sliding, Dashing, WallRunning, Shooting, Standing
    }

    [SerializeField] private MovementState state;
    [SerializeField] private bool sliding;
    [SerializeField] private bool dashing;
    [SerializeField] private bool wallRunning;
    [SerializeField] private bool shooting;
    [SerializeField] private bool isCrouching = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        standingHeight = transform.localScale.y;
        playerCamera = cameraHolder.GetComponent<Camera>();
        keybindManager = keyBindMenu.GetComponent<KeybindManager>();
        wallRun = GetComponent<WallRun>();

        fovSlider.value = fovSlider.minValue;
    }

    private void FixedUpdate()
    {
        PlayerMove();
        normalFov = fovSlider.value;
    }

    private void Update()
    {
        onGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f);
        onJumpPad = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, jumpPad);

        MyInput();
        SpeedControl();
        StateHandle();

        if (state == MovementState.Walking || state == MovementState.Sprinting || state == MovementState.Crouching)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if (Input.GetKeyDown(keybindManager.GetKeyCode("Crouch")))
        {
            isCrouching = !isCrouching;
        }

        if (isCrouching)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
            if (onGround)
            {
                rb.AddForce(Vector3.down * 5f, ForceMode.Force);
            }
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, standingHeight, transform.localScale.z);
        }

        HandleJumpPad();
    }

    private void MyInput()
    {
        horizontal = 0;
        vertical = 0;

        // Forward and backward
        if (Input.GetKey(keybindManager.GetKeyCode("Forward")))
        {
            vertical += 1;
        }
        if (Input.GetKey(keybindManager.GetKeyCode("Backwards")))
        {
            vertical -= 1;
        }

        // Left and right
        if (Input.GetKey(keybindManager.GetKeyCode("Left")))
        {
            horizontal -= 1;
        }
        if (Input.GetKey(keybindManager.GetKeyCode("Right")))
        {
            horizontal += 1;
        }

        WallSlideDown();

        // Jump
        if (Input.GetKey(keybindManager.GetKeyCode("Jump")) && readyToJump && onGround)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCoolDown);
        }
    }

    [SerializeField] private MovementState lastState;

    private void StateHandle()
    {
        if (shooting)
        {
            state = MovementState.Shooting;
            desireMoveSpeed = knockBackSpeed;
            speedIncreaseMultiplier = knockBackSpeedChangeFactor;
            cam.DOFOV(shootingFovChange + normalFov);
            cam.DOTilt(0f);
        }
        else if (wallRunning)
        {
            state = MovementState.WallRunning;
            desireMoveSpeed = wallRunSpeed;
            cam.DOFOV(wallRunFovChange + normalFov);

            Vector3 camLocalPosition = cam.transform.localPosition;

            if (wallRun.LeftWall)
            {
                cam.DOTilt(5f);
            }
            else
            {
                cam.DOTilt(-5f);
            }
        }
        else if (dashing)
        {
            state = MovementState.Dashing;
            desireMoveSpeed = dashSpeed;
            speedIncreaseMultiplier = dashSpeedIncreaseMultiplier;
            cam.DOFOV(dashFovChange + normalFov);
            cam.DOTilt(0f);
        }
        else if (sliding)
        {
            state = MovementState.Sliding;
            slideFovChange += Time.deltaTime * 4f;

            if (OnSlope() && rb.velocity.y < 0.1)
            {
                cam.DOFOV(slideFovChange + normalFov);
                desireMoveSpeed = slideSpeed;
            }
            else
            {
                desireMoveSpeed = sprintSpeed;
                cam.DOFOV(normalFov);
                slideFovChange = 5f;
            }
            speedIncreaseMultiplier = slideSpeedChangeFactor;
        }
        // Crouching Mode
        else if (isCrouching && onGround)
        {
            state = MovementState.Crouching;
            desireMoveSpeed = crouchSpeed;
            cam.DOFOV(normalFov);
            cam.DOTilt(0f);
        }
        // Sprinting Mode
        else if (onGround && Input.GetKey(keybindManager.GetKeyCode("Sprint")))
        {
            state = MovementState.Sprinting;
            desireMoveSpeed = sprintSpeed;
            cam.DOFOV(normalFov + sprintFovChange);
            cam.DOTilt(0f);
            if (dashing)
            {
                state = MovementState.Dashing;
                desireMoveSpeed = dashSpeed;
                speedIncreaseMultiplier = dashSpeedIncreaseMultiplier;
                cam.DOFOV(dashFovChange + normalFov);
                cam.DOTilt(0f);
            }
        }
        // Walking Mode
        else if (onGround)
        {
            state = MovementState.Walking;
            desireMoveSpeed = walkSpeed;
            cam.DOFOV(normalFov);
            cam.DOTilt(0f);
            if (dashing)
            {
                state = MovementState.Dashing;
                desireMoveSpeed = dashSpeed;
                speedIncreaseMultiplier = dashSpeedIncreaseMultiplier;
                cam.DOFOV(dashFovChange + normalFov);
                cam.DOTilt(0f);
            }
        }
        // In Air
        else
        {
            state = MovementState.Air;
            cam.DOFOV(normalFov);
            cam.DOTilt(0f);

            if (desireMoveSpeed < sprintSpeed)
            {
                desireMoveSpeed = walkSpeed;
            }
            else if (dashing)
            {
                state = MovementState.Dashing;
                desireMoveSpeed = dashSpeed;
                speedIncreaseMultiplier = dashSpeedIncreaseMultiplier;
                cam.DOFOV(dashFovChange + normalFov);
                cam.DOTilt(0f);
            }
            else
            {
                desireMoveSpeed = sprintSpeed;
            }

            // Double jump
            if (Input.GetKeyDown(keybindManager.GetKeyCode("Jump")) && readyToDoubleJump)
            {
                DoubleJump();
                readyToDoubleJump = false;
            }
        }

        if (Mathf.Abs(desireMoveSpeed - lastDesireMoveSpeed) > (sprintSpeed - crouchSpeed) && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desireMoveSpeed;
        }

        lastDesireMoveSpeed = desireMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desireMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desireMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeAngleIncrease;
            }
            else
            {
                time += Time.deltaTime * speedIncreaseMultiplier;
            }

            yield return null;
        }

        moveSpeed = desireMoveSpeed;
    }

    private void PlayerMove()
    {
        moveDirection = orientation.forward * vertical + orientation.right * horizontal;

        // On slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y < 0)
            {
                rb.AddForce(Vector3.down * 100f, ForceMode.Force);
            }
        }
        // On ground
        else if (onGround)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        // In air
        else if (!onGround)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        // Turn off gravity while on slope
        if (!wallRunning)
        {
            rb.useGravity = !OnSlope();
        }
    }

    private void SpeedControl()
    {
        // Limit speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }

        if (maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);
        }
    }

    private void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void DoubleJump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * doubleJumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        readyToDoubleJump = true;
        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private void HandleJumpPad()
    {
        if (onJumpPad)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpPadForce, ForceMode.Impulse);

            cam.DOFOV(jumpPadFovChange + normalFov);

            // Limit max y speed
            if (rb.velocity.y > maxYSpeed)
            {
                rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);
            }
        }
    }

    private void WallSlideDown()
    {
        if (wallRun.FrontWall && Input.GetKey(keybindManager.GetKeyCode("Forward")))
        {
            vertical = 0f;
        }
        if (wallRun.RightWall && Input.GetKey(keybindManager.GetKeyCode("Right")) || wallRun.LeftWall && Input.GetKey(keybindManager.GetKeyCode("Left")))
        {
            horizontal = 0f;
        }
    }

    // Getters and Setters for all fields
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public float WalkSpeed
    {
        get { return walkSpeed; }
        set { walkSpeed = value; }
    }

    public float SprintSpeed
    {
        get { return sprintSpeed; }
        set { sprintSpeed = value; }
    }

    public float DashSpeed
    {
        get { return dashSpeed; }
        set { dashSpeed = value; }
    }

    public float SlideSpeed
    {
        get { return slideSpeed; }
        set { slideSpeed = value; }
    }

    public float WallRunSpeed
    {
        get { return wallRunSpeed; }
        set { wallRunSpeed = value; }
    }

    public float MaxYSpeed
    {
        get { return maxYSpeed; }
        set { maxYSpeed = value; }
    }

    public float KnockBackSpeed
    {
        get { return knockBackSpeed; }
        set { knockBackSpeed = value; }
    }

    public float DashSpeedIncreaseMultiplier
    {
        get { return dashSpeedIncreaseMultiplier; }
        set { dashSpeedIncreaseMultiplier = value; }
    }

    public float SlopeIncreaseMultiplier
    {
        get { return slopeIncreaseMultiplier; }
        set { slopeIncreaseMultiplier = value; }
    }

    public float SlideSpeedChangeFactor
    {
        get { return slideSpeedChangeFactor; }
        set { slideSpeedChangeFactor = value; }
    }

    public float KnockBackSpeedChangeFactor
    {
        get { return knockBackSpeedChangeFactor; }
        set { knockBackSpeedChangeFactor = value; }
    }

    public float JumpForce
    {
        get { return jumpForce; }
        set { jumpForce = value; }
    }

    public float JumpCoolDown
    {
        get { return jumpCoolDown; }
        set { jumpCoolDown = value; }
    }

    public float AirMultiplier
    {
        get { return airMultiplier; }
        set { airMultiplier = value; }
    }

    public float JumpPadCheckDistance
    {
        get { return jumpPadCheckDistance; }
        set { jumpPadCheckDistance = value; }
    }

    public bool ReadyToJump
    {
        get { return readyToJump; }
        set { readyToJump = value; }
    }

    public float DoubleJumpForce
    {
        get { return doubleJumpForce; }
        set { doubleJumpForce = value; }
    }

    public bool ReadyToDoubleJump
    {
        get { return readyToDoubleJump; }
        set { readyToDoubleJump = value; }
    }

    public float CrouchSpeed
    {
        get { return crouchSpeed; }
        set { crouchSpeed = value; }
    }

    public float CrouchHeight
    {
        get { return crouchHeight; }
        set { crouchHeight = value; }
    }

    public float PlayerHeight
    {
        get { return playerHeight; }
        set { playerHeight = value; }
    }

    public LayerMask Ground
    {
        get { return ground; }
        set { ground = value; }
    }

    public float GroundDrag
    {
        get { return groundDrag; }
        set { groundDrag = value; }
    }

    public bool OnGround
    {
        get { return onGround; }
        set { onGround = value; }
    }

    public LayerMask JumpPad
    {
        get { return jumpPad; }
        set { jumpPad = value; }
    }

    public float JumpPadForce
    {
        get { return jumpPadForce; }
        set { jumpPadForce = value; }
    }

    public bool OnJumpPad
    {
        get { return onJumpPad; }
        set { onJumpPad = value; }
    }

    public float MaxSlopeAngle
    {
        get { return maxSlopeAngle; }
        set { maxSlopeAngle = value; }
    }

    public Transform Orientation
    {
        get { return orientation; }
        set { orientation = value; }
    }

    public GameObject PlayerObject
    {
        get { return playerObject; }
        set { playerObject = value; }
    }

    public GameObject CameraHolder
    {
        get { return cameraHolder; }
        set { cameraHolder = value; }
    }

    public GameObject KeyBindMenu
    {
        get { return keyBindMenu; }
        set { keyBindMenu = value; }
    }

    public playercamera Cam
    {
        get { return cam; }
        set { cam = value; }
    }

    public Camera PlayerCamera
    {
        get { return playerCamera; }
        set { playerCamera = value; }
    }

    public WallRun WallRun
    {
        get { return wallRun; }
        set { wallRun = value; }
    }

    public KeybindManager KeybindManager
    {
        get { return keybindManager; }
        set { keybindManager = value; }
    }

    public float NormalFov
    {
        get { return normalFov; }
        set { normalFov = value; }
    }

    public float JumpPadFovChange
    {
        get { return jumpPadFovChange; }
        set { jumpPadFovChange = value; }
    }

    public float DashFovChange
    {
        get { return dashFovChange; }
        set { dashFovChange = value; }
    }

    public float WallRunFovChange
    {
        get { return wallRunFovChange; }
        set { wallRunFovChange = value; }
    }

    public float ShootingFovChange
    {
        get { return shootingFovChange; }
        set { shootingFovChange = value; }
    }

    public float SprintFovChange
    {
        get { return sprintFovChange; }
        set { sprintFovChange = value; }
    }

    public float SlideFovChange
    {
        get { return slideFovChange; }
        set { slideFovChange = value; }
    }

    public Slider FOVSlider
    {
        get { return fovSlider; }
        set { fovSlider = value; }
    }

    public float Horizontal
    {
        get { return horizontal; }
        set { horizontal = value; }
    }

    public float Vertical
    {
        get { return vertical; }
        set { vertical = value; }
    }

    public Rigidbody Rb
    {
        get { return rb; }
        set { rb = value; }
    }

    public MovementState State
    {
        get { return state; }
        set { state = value; }
    }

    public bool Sliding
    {
        get { return sliding; }
        set { sliding = value; }
    }

    public bool Dashing
    {
        get { return dashing; }
        set { dashing = value; }
    }

    public bool WallRunning
    {
        get { return wallRunning; }
        set { wallRunning = value; }
    }

    public bool Shooting
    {
        get { return shooting; }
        set { shooting = value; }
    }

    public bool IsCrouching
    {
        get { return isCrouching; }
        set { isCrouching = value; }
    }
}
