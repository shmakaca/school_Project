using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerCam;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject keyBindMenu;
    [SerializeField] private KeybindManager keybindManager;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Dash")]

    [SerializeField] private float dashForce;
    [SerializeField] private float dashUpwardForce;
    [SerializeField] private float maxDashYSpeed;
    [SerializeField] private float dashDuration;

    [Header("Dash CoolDown")]

    [SerializeField] private float dashCoolDown;
    private float dashCoolDownTimer;

    [Header("Settings")]
    [SerializeField] private bool useCameraForward = true;
    [SerializeField] private bool allowAllDirections = true;
    [SerializeField] private bool disableGravity = false;
    [SerializeField] private bool resetVel = true;

    public Transform Orientation
    {
        get { return orientation; }
    }
    public Transform PlayerCam
    {
        get { return playerCam; }
    }
    public GameObject PlayerObject
    {
        get { return playerObject; }
    }
    public Rigidbody RB
    {
        get { return rb; }
    }
    public GameObject KeyBindMenu
    {
        get { return keyBindMenu; }
    }
    public KeybindManager KeybindManager
    {
        get { return keybindManager; }
    }
    public PlayerMovement PlayerMovement
    {
        get { return playerMovement; }
    }
    public float DashForce
    {
        get { return dashForce; }
        private set { dashForce = value; }
    }
    public float DashUpwardForce
    {
        get { return dashUpwardForce; }
        private set { dashUpwardForce = value; }
    }
    public float MaxDashYSpeed
    {
        get { return maxDashYSpeed; }
        private set { maxDashYSpeed = value; }
    }
    public float DashDuration
    {
        get { return dashDuration; }
        private set { dashDuration = value; }
    }
    public float DashCoolDown
    {
        get { return dashCoolDown; }
        private set { dashCoolDown = value; }
    }
    public float DashCoolDownTimer
    {
        get { return dashCoolDownTimer; }
        private set { dashCoolDownTimer = value; }
    }
    public bool UseCameraForward
    {
        get { return useCameraForward; }
        private set { useCameraForward = value; }
    }
    public bool AllowAllDirections
    {
        get { return allowAllDirections; }
        private set { allowAllDirections = value; }
    }
    public bool DisableGravity
    {
        get { return disableGravity; }
        private set { disableGravity = value; }
    }
    public bool ResetVel
    {
        get { return resetVel; }
        private set { resetVel = value; }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();

        keybindManager = KeyBindMenu.GetComponent<KeybindManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeybindManager.GetKeyCode("Dash")))
        {
            DashMovement();
        }

        if (DashCoolDownTimer > 0)
            DashCoolDownTimer -= Time.deltaTime;
    }

    private void DashMovement()
    {
        if (DashCoolDownTimer > 0)
            return;
        else
            DashCoolDownTimer = DashCoolDown;

        PlayerMovement.Dashing = true;
        PlayerMovement.MaxYSpeed = MaxDashYSpeed;

        Transform forwardT;

        if (UseCameraForward)
            forwardT = PlayerCam;
        else
            forwardT = Orientation;

        Vector3 direction = GetDirection(forwardT);
        Vector3 forceToApply = direction * DashForce + Orientation.up * DashUpwardForce;

        DelayForceToApply = forceToApply;
        Invoke(nameof(DelayDashForce), 0.025f);
        Invoke(nameof(ResetDash), DashDuration);
    }

    private Vector3 DelayForceToApply;
    private void DelayDashForce()
    {
        RB.AddForce(DelayForceToApply, ForceMode.Impulse);
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

        Vector3 direction = Vector3.zero;

        if (AllowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }
}
