using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float MouseSens = 100f;

    public Transform playerBody;

    float Xrotate = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float MouseX = Input.GetAxis("Mouse X") * MouseSens * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * MouseSens * Time.deltaTime;

        Xrotate -= MouseY;
        Xrotate = Mathf.Clamp(Xrotate,-90f, 90f);

        transform.localRotation = Quaternion.Euler(Xrotate, 0f, 0f);
        playerBody.Rotate(Vector3.up * MouseX);
 
        
    }
}
    