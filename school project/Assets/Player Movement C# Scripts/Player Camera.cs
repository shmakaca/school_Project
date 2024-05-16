using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class playercamera : MonoBehaviour
{
    private MouseSettings mouseSettings;

    public Transform orientation;
    public Transform cameraHolder;
    public GameObject mouseMenu;

    float xRotation;
    float yRotation;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseSettings = mouseMenu.GetComponent<MouseSettings>();


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

    public void DOFOV(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.5f);
    }

    public void DOTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.5f);
    }

}
