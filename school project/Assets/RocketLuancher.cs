using UnityEngine;

public class RocketLauncher : Weapon
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private Transform grenadeSpawnPoint;
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionForce = 700f;
    [SerializeField] private float explosionDamage = 100f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private LayerMask damageableLayers;

    public Transform playerCamera;
    private Rigidbody playerRigidbody;

    private new void Start()
    {
        base.Start();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    protected override void Shoot()
    {
        // Instantiate the grenade only when shooting
        GameObject grenadeInstance = Instantiate(grenadePrefab, grenadeSpawnPoint.position, playerCamera.rotation);

        // Ignore collision with all GameObjects tagged as "Player"
        Collider grenadeCollider = grenadeInstance.GetComponent<Collider>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Collider playerCollider = player.GetComponent<Collider>();
            if (playerCollider != null)
            {
                Physics.IgnoreCollision(grenadeCollider, playerCollider);
            }
        }

        // Set grenade parameters and activate it
        Grenade grenadeScript = grenadeInstance.GetComponent<Grenade>();
        if (grenadeScript != null)
        {
            grenadeScript.explosionRadius = explosionRadius;
            grenadeScript.explosionForce = explosionForce;
            grenadeScript.explosionDamage = explosionDamage;
            grenadeScript.damageableLayers = damageableLayers;
            grenadeScript.explosionEffect = ImpactHitEffect;
            grenadeScript.ActivateGrenade();
            grenadeScript.Autoexplode();
        }

        Rigidbody grenadeRigidbody = grenadeInstance.GetComponent<Rigidbody>();

        Vector3 forceDirection = playerCamera.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, 100f))
        {
            forceDirection = (hit.point - grenadeSpawnPoint.position).normalized;
        }

        Vector3 throwForceToAdd = forceDirection * launchForce;

        grenadeRigidbody.AddForce(throwForceToAdd, ForceMode.Impulse);

        KnockBack();
    }

    private new void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeybindManager.GetKeyCode("Shoot")))
        {
            HandleShooting();
        }
    }

    private void KnockBack()
    {
        if (playerRigidbody != null && playerCamera != null)
        {
            Vector3 forceDirection = -playerCamera.transform.forward;
            forceDirection.Normalize();
            playerRigidbody.AddForce(forceDirection * knockbackForce, ForceMode.Impulse);
        }
    }
}