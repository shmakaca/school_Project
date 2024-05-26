using UnityEngine;

public class WeaponCameraSetup : MonoBehaviour
{
    public Camera playerCamera;
    public Camera weaponCamera;

    void Start()
    {
        // Ensure the weapon camera follows the main camera's position and rotation
        weaponCamera.transform.SetParent(playerCamera.transform);

        // Set the depth of the weapon camera higher than the main camera
        weaponCamera.depth = playerCamera.depth + 1;

        // Only render the Weapons layer
        weaponCamera.cullingMask = LayerMask.GetMask("Weapons");
        playerCamera.cullingMask &= ~LayerMask.GetMask("Weapons");
    }
}
