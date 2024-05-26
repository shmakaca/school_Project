using UnityEngine;
using DG.Tweening;

public class playercamera : MonoBehaviour
{
    private MouseSettings mouseSettings;

    public Transform orientation;
    public Transform cameraHolder;
    public GameObject MouseMenuSaveChanges;
    public Camera weaponCamera;  // Reference to the weapon camera

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseSettings = MouseMenuSaveChanges.GetComponent<MouseSettings>();

        // Apply the current sensitivity settings at startup
        ApplySensitivitySettings();

        // Sync the initial FOV
        weaponCamera.fieldOfView = GetComponent<Camera>().fieldOfView;
    }

    private void Update()
    {
        // Get the current sensitivity
        Vector2 sensitivity = mouseSettings.GetCurrentSensitivity();

        // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity.x;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity.y;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void ApplySensitivitySettings()
    {
        Vector2 sensitivity = mouseSettings.GetCurrentSensitivity();
        // Use the sensitivity to initialize any necessary settings
        // For example, initializing mouse sensitivity in other parts of your game if needed
    }

    public void DOFOV(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.5f);
        weaponCamera.DOFieldOfView(endValue, 0.5f);  // Apply FOV to weapon camera as well
    }

    public void DOTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.5f);
    }
}
