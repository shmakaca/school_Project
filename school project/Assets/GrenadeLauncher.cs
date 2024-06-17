using UnityEngine;
using UnityEngine.UI;

public class GrenadeLauncher : Weapon
{
    public GameObject grenadePrefab;
    public Transform launchPoint;
    private KeybindManager keybindManager;
    public GameObject keybindManagerObject;
    public Canvas playerUI;

    private new void Start()
    {
        base.Start();
        if (keybindManagerObject != null)
        {
            keybindManager = keybindManagerObject.GetComponent<KeybindManager>();
        }
        else
        {
            Debug.LogError("KeybindManagerObject is not assigned in the inspector.");
        }
    }

    private new void Update()
    {
        base.Update();

        if (keybindManager != null && Input.GetKeyDown(keybindManager.GetKeyCode("Shoot")))
        {
            HandleShooting();
        }
    }

    protected override void Shoot()
    {
        if (grenadePrefab != null && launchPoint != null)
        {
            GameObject grenade = Instantiate(grenadePrefab, launchPoint.position, launchPoint.rotation);

            if (playerUI != null)
            {
                RectTransform uiRectTransform = playerUI.GetComponent<RectTransform>();
                if (uiRectTransform != null)
                {
                    Vector3 uiCenter = uiRectTransform.position;
                    Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(uiCenter.x, uiCenter.y, Camera.main.nearClipPlane));
                    Vector3 direction = (targetPosition - launchPoint.position).normalized;

                    Rigidbody rb = grenade.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        Grenade grenadeComponent = grenade.GetComponent<Grenade>();
                        if (grenadeComponent != null)
                        {
                            grenadeComponent.Launcher = this; // Pass the launcher reference
                            rb.velocity = direction * grenadeComponent.speed;
                        }
                        else
                        {
                            Debug.LogWarning("Grenade component is missing!");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Rigidbody component is missing on the grenade!");
                    }
                }
                else
                {
                    Debug.LogWarning("RectTransform component is missing on player UI!");
                }
            }
            else
            {
                Debug.LogWarning("Player UI is missing!");
            }
        }
        else
        {
            Debug.LogWarning("Grenade prefab or launch point is missing!");
        }
    }
}
