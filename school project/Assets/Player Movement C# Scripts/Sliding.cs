using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerObject;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject keyBindMenu;
    [SerializeField] private KeybindManager keybindManager;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Sliding")]
    [SerializeField] private float maxSlideTime;
    [SerializeField] private float slideForce;
    [SerializeField] private float slideTimer;
    [SerializeField] private float slideHeight;
    [SerializeField] private float standingHeight;
    [SerializeField] private float slideKeyHoldTime;
    [SerializeField] private float slideHoldDuration;

    [Header("Camera Effects")]
    [SerializeField] private playercamera camera;
    [SerializeField] private float slideFov;

    [Header("Input")]
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask ground;
    [SerializeField] private bool onGround;

    public Transform Orientation
    {
        get { return orientation; }
        set { orientation = value; }
    }
    public Transform PlayerObject
    {
        get { return playerObject; }
        set { playerObject = value; }
    }
    public Rigidbody RB
    {
        get { return rb; }
        set { rb = value; }
    }
    public GameObject KeyBindMenu
    {
        get { return keyBindMenu; }
        set { keyBindMenu = value; }
    }
    public KeybindManager KeybindManager
    {
        get { return keybindManager; }
        set { keybindManager = value; }
    }
    public PlayerMovement PlayerMovement
    {
        get { return playerMovement; }
        set { playerMovement = value; }
    }
    public float MaxSlideTime
    {
        get { return maxSlideTime; }
        private set { maxSlideTime = value; }
    }
    public float SlideForce
    {
        get { return slideForce; }
        set { slideForce = value; }
    }
    public float SlideHeight
    {
        get { return slideHeight; }
        set { slideHeight = value; }
    }
    public float SlideHoldDuration
    {
        get { return slideHoldDuration; }
        set { slideHoldDuration = value; }
    }
    public playercamera Camera
    {
        get { return camera; }
        set { camera = value; }
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
    public bool OnGround
    {
        get { return onGround; }
        set { onGround = value; }
    }

    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        PlayerMovement = GetComponent<PlayerMovement>();
        standingHeight = PlayerObject.localScale.y;
        slideFov = PlayerMovement.NormalFov + 5f;

        KeybindManager = KeyBindMenu.GetComponent<KeybindManager>();
    }

    private void FixedUpdate()
    {
        if (PlayerMovement.Sliding)
        {
            SlideMovement();
        }
    }

    private void Update()
    {
        OnGround = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.3f);

        Horizontal = PlayerMovement.Horizontal;
        Vertical = PlayerMovement.Vertical;

        if ((Horizontal != 0 || Vertical != 0) && OnGround && !PlayerMovement.Sliding)
        {
            if (Input.GetKey(KeybindManager.GetKeyCode("Slide")))
            {
                slideKeyHoldTime += Time.deltaTime;
                if (slideKeyHoldTime >= SlideHoldDuration)
                {
                    StartSlide();
                }
            }
            else
            {
                slideKeyHoldTime = 0f;
            }
        }

        if (Input.GetKeyUp(KeybindManager.GetKeyCode("Slide")) && PlayerMovement.Sliding)
        {
            StopSlide();
        }
    }

    private void StartSlide()
    {
        PlayerMovement.Sliding = true;
        PlayerObject.localScale = new Vector3(PlayerObject.localScale.x, SlideHeight, PlayerObject.localScale.z);

        RB.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = MaxSlideTime;
        slideKeyHoldTime = 0f; // Reset the hold time after starting the slide
    }

    private void SlideMovement()
    {
        Vector3 direction = Orientation.forward * Vertical + Orientation.right * Horizontal;

        // Sliding on ground
        if (!PlayerMovement.OnSlope() || RB.velocity.y > -0.1f)
        {
            RB.AddForce(direction.normalized * SlideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }
        // Sliding on slope
        else
        {
            RB.AddForce(PlayerMovement.GetSlopeMoveDirection(direction) * SlideForce, ForceMode.Force);
        }

        if (slideTimer <= 0)
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        PlayerMovement.Sliding = false;
        PlayerObject.localScale = new Vector3(PlayerObject.localScale.x, standingHeight, PlayerObject.localScale.z);
        Camera.DOFOV(PlayerMovement.NormalFov);
        slideFov = PlayerMovement.NormalFov + 5f;
    }
}
