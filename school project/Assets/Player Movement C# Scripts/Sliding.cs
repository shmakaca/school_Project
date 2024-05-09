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
    private float SlideFov;

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
        SlideFov = PlayerMovement.NormalPov + 5f;
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

        SlieTimer= MaxSlideTime;
        
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
            SlideFov += Time.deltaTime * 5f;

            if (SlideFov >= PlayerMovement.NormalPov + 15f)
                SlideFov = PlayerMovement.NormalPov + 15f;

            if (PlayerMovement.OnSlope())
            {
                Camera.DOFOV(SlideFov);
            }

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
        Camera.DOFOV(PlayerMovement.NormalPov);
        SlideFov = PlayerMovement.NormalPov + 5f;
    }
}
