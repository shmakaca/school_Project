using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class playercamera : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orentation;
    public Transform CameraHolder;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        // get  maouse input

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        CameraHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orentation.rotation = Quaternion.Euler(0, yRotation, 0);

    }

    public void DOFOV(float Endvalue)
    {
        GetComponent<Camera>().DOFieldOfView(Endvalue, 0.25f);
    }

    public void DOTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
}