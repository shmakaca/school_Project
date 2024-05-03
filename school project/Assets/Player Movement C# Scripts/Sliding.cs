using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform Orientation;
    public Transform PlayerObject;
    private Rigidbody rb;
    private PlayerMovement PlayerMovement;

    [Header("Sliding")]
    public float MaxSlideTime;
    public float SlideForce;
    private float SlieTimer;

    public float SlideHieght;
    private float StandingHieght;

    [Header("Camera Effects")]
    public playercamera Camera;
    private float SlideFov = 85f;

    [Header("Input")]
    public KeyCode SlideKey = KeyCode.LeftControl;
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
        StandingHieght = PlayerObject.localScale.y;
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

        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");

        if ((Horizontal != 0 || Vertical != 0) && OnGround && !PlayerMovement.sliding && Input.GetKeyDown(SlideKey))
        {
            StartSlide();
        }

        if (Input.GetKeyUp(SlideKey) && PlayerMovement.sliding)
        {
            StopSlide();
        }

    }
    private void StartSlide()
    {
        PlayerMovement.sliding = true;
        PlayerObject.localScale = new Vector3(PlayerObject.localScale.x, SlideHieght, PlayerObject.localScale.z);

        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        SlideFov += Time.deltaTime * 5f;

        SlieTimer= MaxSlideTime;

        if (SlideFov >= 95f)
            SlideFov = 95f;
        if (PlayerMovement.OnSlope())
        {
            Camera.DOFOV(SlideFov);
        }
        
    }

    private void SlideMovement()
    {
        Vector3 Direction = Orientation.forward * Vertical + Orientation.right * Horizontal;

        // sliding on ground

        if (!PlayerMovement.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(Direction.normalized * SlideForce, ForceMode.Force);

            SlieTimer -= Time.deltaTime;
        }
        // slidng on slope
        else
        {
            rb.AddForce(PlayerMovement.GetSlopeMoveDirection(Direction) * SlideForce, ForceMode.Force);
        }

        if (SlieTimer <= 0)
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        PlayerMovement.sliding = false;
        PlayerObject.localScale = new Vector3(PlayerObject.localScale.x, StandingHieght, PlayerObject.localScale.z);
        Camera.DOFOV(80f);
        SlideFov = 85f;
    }
}
