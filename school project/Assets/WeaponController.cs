using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform playerCamera;
    public Transform weaponTransform;
    public float maxDistance = 2f;
    public float minDistance = 1f;
    public float MaxFov;

    // Update is called once per frame
    void Update()
    {
        // Calculate the desired distance between the camera and weapon based on camera's FOV
        float distance = Mathf.Lerp(minDistance, maxDistance, playerCamera.GetComponent<Camera>().fieldOfView / MaxFov); // 60 is a hypothetical max FOV

        // Set the weapon's position relative to the camera
        weaponTransform.localPosition = new Vector3(0f, 0f, distance);
    }
}
