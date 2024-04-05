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

    public float SlideHieght;
    private float StandingHieght;

    [Header("Input")]
    public KeyCode SlideKey = KeyCode.LeftControl;
    private float KeyPressingTime = 0f;
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
        if(PlayerMovement.sliding)
        {
            SlideMovement();
        }
    }

    private void Update()
    {
        OnGround = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.3f);

        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(SlideKey))
            KeyPressingTime += Time.deltaTime;
        else
        {
            KeyPressingTime = 0f;
        }
        if(KeyPressingTime > 0.15f )
        {

            Input.GetKeyDown(SlideKey);
        }

        if ((Horizontal != 0 || Vertical != 0) && OnGround && KeyPressingTime > 0.15f )
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
    }

    private void SlideMovement()
    {
        Vector3 Direction = Orientation.forward * Vertical + Orientation.right * Horizontal;

        // sliding on ground

        if(!PlayerMovement.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(Direction.normalized * SlideForce, ForceMode.Force);

        }
        // slidng on slope
        else
        {
            rb.AddForce(PlayerMovement.GetSlopeMoveDirection(Direction) * SlideForce, ForceMode.Force);
            KeyPressingTime = 0.15f;
        }

        if (KeyPressingTime > 0.15 + MaxSlideTime && PlayerMovement.sliding)
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        PlayerMovement.sliding = false;
        PlayerObject.localScale = new Vector3(PlayerObject.localScale.x, StandingHieght, PlayerObject.localScale.z);
    }
}
