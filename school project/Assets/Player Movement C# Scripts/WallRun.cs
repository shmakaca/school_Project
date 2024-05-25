using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("Wall Runing")]
    public LayerMask Ground;
    public LayerMask Wall;
    public float WallRunForce;
    public float WallJumpUpForce;
    public float WallJumpSlideForce;
    public float WallCimbSpeed;
    public float WallRunStamina;
    public float WallRunTimer;

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
    public bool RightWall;
    public bool LeftWall;

    [Header("ExitingWall")]
    private bool ExitingWall;
    public float ExitWallTime;
    private float ExitWallTimer;

    [Header("Graviry")]
    public bool UseGarvity;
    public float GravityCounterForce;

    [Header("Refrences")]
    public Transform Orientation;
    private PlayerMovement PlayerMovement;
    private Rigidbody Rb;
    public GameObject KeyBindMenu;
    private KeybindManager KeybindManager;


    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
        PlayerMovement = GetComponent<PlayerMovement>();
        KeybindManager = KeyBindMenu.GetComponent<KeybindManager>();
    }

    private void FixedUpdate()
    {
        if (PlayerMovement.WallRunning)
        {
            WallRunMoveMent();
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

        // State 1 - WallRining
        if ((LeftWall || RightWall) && Vertical > 0 && AboveGround() && !ExitingWall)
        {
            if (!PlayerMovement.WallRunning)
            {
                StartWallRun();
            }

            if (WallRunTimer > 0)
            {
                WallRunTimer -= Time.deltaTime;
            }

            if (WallRunTimer <= 0 && PlayerMovement.WallRunning)
            {
                ExitingWall = true;
                ExitWallTimer = ExitWallTime;
            }

            if (Input.GetKeyDown(KeybindManager.GetKeyCode("Jump")))
            {
                Walljump();
            }
        }

        //State 2 - Exitng Wall Run
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

        else
        {
            if (PlayerMovement.WallRunning)
            {
                StopWallRun();
            }
        }
    }

    private void StartWallRun()
    {
        PlayerMovement.WallRunning = true;

        WallRunTimer = WallRunStamina;

        Rb.velocity = new Vector3(Rb.velocity.x, 0f, Rb.velocity.z);

    }

    private void WallRunMoveMent()
    {
        Rb.useGravity = UseGarvity;


        Vector3 WallNormal = RightWall ? RightWallHit.normal : LeftWallHit.normal;

        Vector3 WallForward = Vector3.Cross(WallNormal, transform.up);

        if ((Orientation.forward - WallForward).magnitude > (Orientation.forward - -WallForward).magnitude)
        {
            WallForward = -WallForward;
        }        // forward force

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

        if (UseGarvity)
        {
            Rb.AddForce(transform.up * GravityCounterForce, ForceMode.Force);
        }

    }

    private void StopWallRun()
    {
        PlayerMovement.WallRunning = false;
    }

    private void Walljump()
    {
        ExitingWall = true;

        ExitWallTimer = ExitWallTime;

        Vector3 WallNormal = RightWall ? RightWallHit.normal : LeftWallHit.normal;

        Vector3 ForceToApply = transform.up * WallJumpUpForce + WallNormal * WallJumpSlideForce;

        Rb.velocity = new Vector3(Rb.velocity.x, 0f, Rb.velocity.z);
        Rb.AddForce(ForceToApply, ForceMode.Impulse);
    }
}

