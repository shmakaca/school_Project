using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("References")]
    public Transform Orientation;
    public Transform PlayerCam;
    private Rigidbody rb;
    private PlayerMovement PlayerMovement;

    [Header("Dash")]
    private float DashForce;
    public float AirDashForce;
    public float GroundDashForce;
    public float GroundDashSpeedChangeFactor;
    public float DashUpwardForce;
    public float DashDuration;


    [Header("DashCoolDown")]
    public float DashCoolDown;
    private float DashCoolDownTimer;

    private float Horizontal;
    private float Vertical;

    [Header("Input")]
    public KeyCode DashKey = KeyCode.E;


    [Header("Ground Check")]
    public float PlayerHeight;
    public LayerMask Ground;
    bool OnGround;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        PlayerMovement = GetComponent<PlayerMovement>();

    }

    private void Update()
    {
        OnGround = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.3f);

        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");

        if (OnGround)
        {
            DashForce = GroundDashForce;
        }
        else
        {
            DashForce = AirDashForce;
        }
        if (Input.GetKeyDown(DashKey))
        {
            dash();
        }

        if (DashCoolDownTimer > 0)
            DashCoolDownTimer -= Time.deltaTime;
    }

    private void dash()
    {
        if (DashCoolDownTimer > 0)
            return;
        else
            DashCoolDownTimer = DashCoolDown;

        PlayerMovement.Dashing = true;

        Vector3 ForceToApply = Orientation.forward * DashForce * Vertical + Orientation.up * DashUpwardForce ;

        rb.AddForce(ForceToApply, ForceMode.Impulse);

        Invoke(nameof(ResetDash), DashDuration);
    }


    private void ResetDash()
    {
        PlayerMovement.Dashing = false;
    }


}
