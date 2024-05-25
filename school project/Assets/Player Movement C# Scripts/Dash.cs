using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("References")]
    public Transform Orientation;
    public Transform PlayerCam;
    public GameObject PlayerObject;
    private Rigidbody rb;
    public GameObject KeyBindMenu;
    private KeybindManager KeybindManager;
    private PlayerMovement PlayerMovement;

    [Header("Dash")]
    public float DashForce;
    public float DashUpwardForce;
    public float MaxDashYSpeed;
    public float DashDuration;

    [Header("Dash CoolDown")]
    public float DashCoolDown;
    private float DashCoolDownTimer;

    private float Horizontal;
    private float Vertical;

    [Header("Settings")]
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;




    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        PlayerMovement = GetComponent<PlayerMovement>();

        KeybindManager = KeyBindMenu.GetComponent<KeybindManager>();

    }

    private void Update()
    {


        if (Input.GetKeyDown(KeybindManager.GetKeyCode("Dash")))
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
        PlayerMovement.MaxYSpeed = MaxDashYSpeed;
 
        Transform forwardT;

        if (useCameraForward)
            forwardT = PlayerCam; /// where you're looking
        else
            forwardT = Orientation; /// where you're facing (no up or down)

        Vector3 direction = GetDirection(forwardT);


        Vector3 ForceToApply = direction * DashForce + Orientation.up * DashUpwardForce;

        DelayForceToApply = ForceToApply;

        Invoke(nameof(DelayDashForce), 0.025f);

        Invoke(nameof(ResetDash), DashDuration);
    }

    private Vector3 DelayForceToApply;
    private void DelayDashForce()
    {
        rb.AddForce(DelayForceToApply, ForceMode.Impulse);
    }
    private void ResetDash()
    {
        PlayerObject.GetComponent<CapsuleCollider>().enabled = true;

        PlayerMovement.Dashing = false;
        PlayerMovement.MaxYSpeed = 0;


    }

    private Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = PlayerMovement.Horizontal;
        float verticalInput = PlayerMovement.Vertical;

        Vector3 direction = new Vector3();

        if (allowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }

}
