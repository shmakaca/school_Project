using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("Wall Running")]
    public LayerMask Ground;
    public LayerMask Wall;
    public float WallRunForce;
    public float WallJumpUpForce;
    public float WallJumpSlideForce;
    public float WallSlideForce;
    public float WallCimbSpeed;
    public float WallRunStamina;
    private float currentWallRunStamina; // Current stamina
    public float WallRunTimer;
    public float StaminaRegenRate; // Stamina regeneration rate per second

    [Header("Input")]
    private bool UpWardsRunning;
    private bool DownWardsRunning;
    private float Horizontal;
    private float Vertical;

    [Header("Detection")]
    public float WallCheckDistance;
    public float MinJumpHeight;
    private RaycastHit LeftWallHit;
    private RaycastHit RightWallHit;
    private RaycastHit FrontWallHit;
    public bool RightWall;
    public bool LeftWall;
    public bool FrontWall;

    [Header("Exiting Wall")]
    private bool ExitingWall;
    public float ExitWallTime;
    private float ExitWallTimer;

    [Header("Gravity")]
    public bool UseGravity;
    public float GravityCounterForce;

    [Header("References")]
    public Transform Orientation;
    private PlayerMovement PlayerMovement;
    private Rigidbody Rb;
    public GameObject KeyBindMenu;
    private KeybindManager KeybindManager;

    [Header("Camera")]
    public playercamera Cam;

    public float WallRunCameraOffset; // New: offset for camera position

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
        PlayerMovement = GetComponent<PlayerMovement>();
        KeybindManager = KeyBindMenu.GetComponent<KeybindManager>();
        currentWallRunStamina = WallRunStamina; // Initialize stamina to max
    }

    private void FixedUpdate()
    {
        if (PlayerMovement.WallRunning)
        {
            WallRunMovement();
        }
        if (FrontWall && Vertical == 1f || LeftWall && Horizontal == -1f || RightWall && Horizontal == 1f)
        {
            SlidingDownTheWall();
        }
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void CheckForWall()
    {
        RightWall = Physics.Raycast(transform.position, Orientation.right, out RightWallHit, WallCheckDistance, Wall);
        LeftWall = Physics.Raycast(transform.position, -Orientation.right, out LeftWallHit, WallCheckDistance, Wall);
        FrontWall = Physics.Raycast(transform.position, Orientation.forward,out FrontWallHit, WallCheckDistance, Wall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, MinJumpHeight, Ground);
    }

    private void StateMachine()
    {
        Horizontal = PlayerMovement.Horizontal;
        Vertical = PlayerMovement.Vertical;

        UpWardsRunning = Input.GetKey(KeybindManager.GetKeyCode("WallRunUp"));
        DownWardsRunning = Input.GetKey(KeybindManager.GetKeyCode("WallRunDown"));

        // State 1 - Wall Running
        if ((LeftWall || RightWall) && Vertical > 0 && AboveGround() && !ExitingWall && currentWallRunStamina > 0)
        {
            if (!PlayerMovement.WallRunning)
            {
                StartWallRun();
            }

            if (WallRunTimer > 0)
            {
                WallRunTimer -= Time.deltaTime;
                currentWallRunStamina -= Time.deltaTime; // Decrease stamina over time
            }

            if (currentWallRunStamina <= 0 && PlayerMovement.WallRunning)
            {
                ExitingWall = true;
                ExitWallTimer = ExitWallTime;
            }

            if (Input.GetKeyDown(KeybindManager.GetKeyCode("Jump")))
            {
                WallJump();
            }
        }

        // State 2 - Exiting Wall Run
        else if (ExitingWall)
        {
            if (PlayerMovement.WallRunning)
            {
                StopWallRun();
            }

            if (ExitWallTimer > 0)
            {
                ExitWallTimer -= Time.deltaTime;
            }

            if (ExitWallTimer <= 0)
            {
                ExitingWall = false;
            }
        }

        // State 3 - Not Wall Running, regenerate stamina
        else
        {
            if (PlayerMovement.WallRunning)
            {
                StopWallRun();
            }

            // Regenerate stamina when not wall running
            if (currentWallRunStamina < WallRunStamina)
            {
                currentWallRunStamina += Time.deltaTime * StaminaRegenRate;
                currentWallRunStamina = Mathf.Clamp(currentWallRunStamina, 0, WallRunStamina);
            }
        }
    }

    private void StartWallRun()
    {
        PlayerMovement.WallRunning = true;

        WallRunTimer = WallRunStamina; // Set the timer to max stamina value

        Rb.velocity = new Vector3(Rb.velocity.x, 0f, Rb.velocity.z);

    }

    private void WallRunMovement()
    {
        Rb.useGravity = UseGravity;

        Vector3 WallNormal = RightWall ? RightWallHit.normal : LeftWallHit.normal;

        Vector3 WallForward = Vector3.Cross(WallNormal, transform.up);

        if ((Orientation.forward - WallForward).magnitude > (Orientation.forward - -WallForward).magnitude)
        {
            WallForward = -WallForward;
        }

        // Forward force
        Rb.AddForce(WallForward * WallRunForce, ForceMode.Force);

        if (UpWardsRunning)
        {
            Rb.velocity = new Vector3(Rb.velocity.x, WallCimbSpeed, Rb.velocity.z);
        }

        if (DownWardsRunning)
        {
            Rb.velocity = new Vector3(Rb.velocity.x, -WallCimbSpeed, Rb.velocity.z);
        }

        if (!(LeftWall && Horizontal > 0) && !(RightWall && Horizontal < 0))
        {
            Rb.AddForce(-WallNormal * 100, ForceMode.Force);
        }

        if (UseGravity)
        {
            Rb.AddForce(transform.up * GravityCounterForce, ForceMode.Force);
        }
    }

    private void StopWallRun()
    {
        PlayerMovement.WallRunning = false;

        // Reset camera after wall run
        Cam.DOFOV(PlayerMovement.NormalFov);
        Cam.DOTilt(0f);
        Cam.transform.localPosition = new Vector3(0, Cam.transform.localPosition.y, Cam.transform.localPosition.z);
    }

    private void WallJump()
    {
        ExitingWall = true;

        ExitWallTimer = ExitWallTime;

        Vector3 WallNormal = RightWall ? RightWallHit.normal : LeftWallHit.normal;

        Vector3 ForceToApply = transform.up * WallJumpUpForce + WallNormal * WallJumpSlideForce;

        Rb.velocity = new Vector3(Rb.velocity.x, 0f, Rb.velocity.z);
        Rb.AddForce(ForceToApply, ForceMode.Impulse);
    }

    private void SlidingDownTheWall()
    {
        RaycastHit wallHit;
        if (RightWall)
        {
            wallHit = RightWallHit;
        }
        else if (LeftWall)
        {
            wallHit = LeftWallHit;
        }
        else if (FrontWall && Vertical == 1f)
        {
            wallHit = FrontWallHit;
        }
        else
        {
            // No valid wall to slide down
            return;
        }

        // Calculate the direction the player should slide down
        Vector3 slideDirection = Vector3.ProjectOnPlane(-wallHit.normal, Vector3.up).normalized;

        // Apply downward force to make the player slide down
        Rb.AddForce(slideDirection * WallSlideForce, ForceMode.Force);
    }

}
