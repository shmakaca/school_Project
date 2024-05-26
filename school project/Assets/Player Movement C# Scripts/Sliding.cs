using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform Orientation;
    public Transform PlayerObject;
    private Rigidbody rb;
    public GameObject KeyBindMenu;
    private KeybindManager KeybindManager;
    private PlayerMovement PlayerMovement;

    [Header("Sliding")]
    public float MaxSlideTime;
    public float SlideForce;
    private float SlideTimer;
    public float SlideHeight;
    private float StandingHeight;
    private float slideKeyHoldTime = 0f;
    public float slideHoldDuration;

    [Header("Camera Effects")]
    public playercamera Camera;
    private float SlideFov;

    [Header("Input")]
    private float Horizontal;
    private float Vertical;

    [Header("Ground Check")]
    public float PlayerHeight;
    public LayerMask Ground;
    bool OnGround;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        PlayerMovement = GetComponent<PlayerMovement>();
        StandingHeight = PlayerObject.localScale.y;
        SlideFov = PlayerMovement.NormalFov + 5f;

        KeybindManager = KeyBindMenu.GetComponent<KeybindManager>();
    }

    private void FixedUpdate()
    {
        if (PlayerMovement.sliding)
        {
            SlideMovement();
        }
    }

    private void Update()
    {
        OnGround = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.3f);

        Horizontal = PlayerMovement.Horizontal;
        Vertical = PlayerMovement.Vertical;

        if ((Horizontal != 0 || Vertical != 0) && OnGround && !PlayerMovement.sliding)
        {
            if (Input.GetKey(KeybindManager.GetKeyCode("Slide")))
            {
                slideKeyHoldTime += Time.deltaTime; 
                if (slideKeyHoldTime >= slideHoldDuration)
                {
                    StartSlide();
                }
            }
            else
            {
                slideKeyHoldTime = 0f; 
            }
        }

        if (Input.GetKeyUp(KeybindManager.GetKeyCode("Slide")) && PlayerMovement.sliding)
        {
            StopSlide();
        }
    }

    private void StartSlide()
    {
        PlayerMovement.sliding = true;
        PlayerObject.localScale = new Vector3(PlayerObject.localScale.x, SlideHeight, PlayerObject.localScale.z);

        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        SlideTimer = MaxSlideTime;
        slideKeyHoldTime = 0f; // Reset the hold time after starting the slide
    }

    private void SlideMovement()
    {
        Vector3 direction = Orientation.forward * Vertical + Orientation.right * Horizontal;

        // Sliding on ground
        if (!PlayerMovement.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(direction.normalized * SlideForce, ForceMode.Force);

            SlideTimer -= Time.deltaTime;
        }
        // Sliding on slope
        else
        {
            rb.AddForce(PlayerMovement.GetSlopeMoveDirection(direction) * SlideForce, ForceMode.Force);
        }

        if (SlideTimer <= 0)
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        PlayerMovement.sliding = false;
        PlayerObject.localScale = new Vector3(PlayerObject.localScale.x, StandingHeight, PlayerObject.localScale.z);
        Camera.DOFOV(PlayerMovement.NormalFov);
        SlideFov = PlayerMovement.NormalFov + 5f;
    }
}
