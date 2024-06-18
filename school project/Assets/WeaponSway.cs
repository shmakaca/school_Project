using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] public Transform weaponTransform;

    [Header("Sway Properties")]
    [SerializeField] private float swayAmount = 0.03f;
    [SerializeField] public float maxSwayAmount = 0.5f;
    [SerializeField] public float swaySmooth = 10f;
    [SerializeField] public AnimationCurve swayCurve;
    [Range(0f, 1f)]
    [SerializeField] public float swaySmoothCounteraction = 0.9f;

    [Header("Rotation")]
    [SerializeField] public float rotationSwayMultiplier = -2f;

    [Header("Position")]
    [SerializeField] public float positionSwayMultiplier = 4f;

    [Header("Bobbing")]
    [SerializeField] public float IdleBobFrequency = 3f;
    [SerializeField] public float IdlebobAmplitude = 0.2f;

    [SerializeField] public float WalkingBobFrequency = 3.5f;
    [SerializeField] public float WalkingbobAmplitude = 0.2f;

    [SerializeField] public float RunningBobFrequency = 4f;
    [SerializeField] public float RunningbobAmplitude = 0.5f;

    [SerializeField] public float WallRunningBobFrequency = 4.5f;
    [SerializeField] public float WallRunningbobAmplitude = 1f;

    [SerializeField] private float BobFrequency;
    [SerializeField] private float bobAmplitude;
    [SerializeField] private float bobbingOffset;

    [Header("Recoil")]
    [SerializeField] public float recoilAmount = 0.1f;
    [SerializeField] public float recoilRecoverySpeed = 2f;
    [SerializeField] public float recoilRotationAmount = 10f;

    [Header("Idle Sway")]
    [SerializeField] public float idleSwayAmount = 0.01f;
    [SerializeField] public float idleSwaySpeed = 1f;

    [Header("Tilt")]
    [SerializeField] public float sprintTiltAngle = 10f;
    [SerializeField] public float WallRunTiltAngle = 10f;
    [SerializeField] private float sprintTilt;

    [Header("References")]
    [SerializeField] PlayerMovement PlayerMovement;
    [SerializeField] WallRun WallRun;
    [SerializeField] GameObject Player;

    public bool DisableSway = false;
    private bool isReloading = false;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector2 sway;
    private Quaternion lastRot;
    private float bobbingTime;
    private Vector3 recoilOffset;
    private Quaternion recoilRotation;
    private bool isHiding = false;
    private bool isShowing = false;
    private float hideShowProgress = 0f;

    private void Reset()
    {
        Keyframe[] ks = new Keyframe[] { new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0) };
        swayCurve = new AnimationCurve(ks);
    }

    private void Start()
    {
        PlayerMovement = Player.GetComponent<PlayerMovement>();
        WallRun = Player.GetComponent<WallRun>();

        if (!weaponTransform)
            weaponTransform = transform;

        lastRot = transform.localRotation;
        initialPosition = weaponTransform.localPosition;
        initialRotation = weaponTransform.localRotation;
    }

    private void Update()
    {
        if (DisableSway || isReloading) return;

        BobAdjustment();
        TiltAdjustment();

        var angularVelocity = Quaternion.Inverse(lastRot) * transform.rotation;
        float mouseX = FixAngle(angularVelocity.eulerAngles.y) * swayAmount;
        float mouseY = -FixAngle(angularVelocity.eulerAngles.x) * swayAmount;
        lastRot = transform.rotation;
        sway = Vector2.MoveTowards(sway, Vector2.zero, swayCurve.Evaluate(Time.deltaTime * swaySmoothCounteraction * sway.magnitude * swaySmooth));
        sway = Vector2.ClampMagnitude(new Vector2(mouseX, mouseY) + sway, maxSwayAmount);

        // Idle Sway Calculation
        float idleSwayOffsetX = Mathf.Sin(Time.time * idleSwaySpeed) * idleSwayAmount;
        float idleSwayOffsetY = Mathf.Cos(Time.time * idleSwaySpeed) * idleSwayAmount;

        // Recoil Calculation
        recoilOffset = Vector3.Lerp(recoilOffset, Vector3.zero, Time.deltaTime * recoilRecoverySpeed);
        recoilRotation = Quaternion.Lerp(recoilRotation, Quaternion.identity, Time.deltaTime * recoilRecoverySpeed);

        // Applying Transformations
        Vector3 newPosition = Vector3.Lerp(weaponTransform.localPosition, new Vector3(sway.x + idleSwayOffsetX, sway.y + idleSwayOffsetY + bobbingOffset, 0) * positionSwayMultiplier * Mathf.Deg2Rad + initialPosition + recoilOffset, swayCurve.Evaluate(Time.deltaTime * swaySmooth));
        Quaternion newRotation = Quaternion.Slerp(weaponTransform.localRotation, initialRotation * Quaternion.Euler(Mathf.Rad2Deg * rotationSwayMultiplier * new Vector3(-sway.y, sway.x, 0) + new Vector3(0, 0, -sprintTilt)) * recoilRotation, swayCurve.Evaluate(Time.deltaTime * swaySmooth));

        weaponTransform.localPosition = newPosition;
        weaponTransform.localRotation = newRotation;
    }

    private float FixAngle(float angle)
    {
        return Mathf.Repeat(angle + 180f, 360f) - 180f;
    }

    // Method to trigger recoil
    public void TriggerRecoil()
    {
        recoilOffset += Vector3.back * recoilAmount;
        recoilRotation *= Quaternion.Euler(Vector3.left * recoilRotationAmount);
    }

    private void BobAdjustment()
    {
        if (PlayerMovement.State == PlayerMovement.MovementState.Walking)
        {
            bobbingTime += Time.deltaTime * WalkingBobFrequency;
            bobbingOffset = Mathf.Sin(bobbingTime) * WalkingbobAmplitude;
        }
        else if (PlayerMovement.State == PlayerMovement.MovementState.WallRunning)
        {
            bobbingTime += Time.deltaTime * WallRunningBobFrequency;
            bobbingOffset = Mathf.Sin(bobbingTime) * WallRunningbobAmplitude;
        }
        else if (PlayerMovement.State == PlayerMovement.MovementState.Sprinting)
        {
            bobbingTime += Time.deltaTime * RunningBobFrequency;
            bobbingOffset = Mathf.Sin(bobbingTime) * RunningbobAmplitude;
        }
        else
        {
            bobbingTime += Time.deltaTime * IdleBobFrequency;
            bobbingOffset = Mathf.Sin(bobbingTime) * IdlebobAmplitude;
        }
    }

    private void TiltAdjustment()
    {
        if (PlayerMovement.State == PlayerMovement.MovementState.Sprinting)
        {
            sprintTilt = sprintTiltAngle;
        }
        else if (WallRun.LeftWall && PlayerMovement.State == PlayerMovement.MovementState.WallRunning)
        {
            sprintTilt = WallRunTiltAngle;
        }
        else if (WallRun.RightWall && PlayerMovement.State == PlayerMovement.MovementState.WallRunning)
        {
            sprintTilt = -WallRunTiltAngle;
        }
        else
        {
            sprintTilt = 0;
        }
    }

}
