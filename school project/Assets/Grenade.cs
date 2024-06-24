using UnityEngine;
using System.Collections;

public class Grenade : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    public float explosionRadius;
    public float explosionForce;
    public float explosionDamage;
    public float TimeToExplodeAftercollide;
    public float AutoExplodeTime;
    private bool hasExploded;
    public LayerMask damageableLayers;

    // Add a reference to the explosion effect prefab
    public GameObject explosionEffect;
    public Collider playerCollider; // Reference to the player's collider
    public Rigidbody playerRigidbody; // Reference to the player's Rigidbody
    public float disableGravityDuration = 2f; // Duration to disable gravity and drag

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // Disable physics initially
        rb.isKinematic = true;
        col.enabled = false; // Disable the collider initially
    }

    public void ActivateGrenade()
    {
        rb.isKinematic = false;
        col.enabled = true; // Enable the collider when activated
        Debug.Log("Grenade Activated: Rigidbody and Collider enabled");

        // Ignore collision with the player
        if (playerCollider != null)
        {
            Physics.IgnoreCollision(col, playerCollider);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasExploded)
        {
            StartCoroutine(AfterCollideExplode());
            hasExploded = true;
        }
    }

    IEnumerator AfterCollideExplode()
    {
        yield return new WaitForSeconds(TimeToExplodeAftercollide);

        Explode();
    }

    IEnumerator AutoExplode()
    {
        yield return new WaitForSeconds(AutoExplodeTime);

        Explode();
    }

    public void Autoexplode()
    {
        StartCoroutine(AutoExplode());
    }

    private void Explode()
    {
        Vector3 explosionPoint = transform.position;

        // Instantiate the explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, explosionPoint, Quaternion.identity);
        }

        Collider[] colliders = Physics.OverlapSphere(explosionPoint, explosionRadius, damageableLayers);

        // Apply damage and force to objects within the radius
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, explosionPoint, explosionRadius);
            }

            // If the player is within the explosion radius, disable gravity and drag
            if (nearbyObject == playerCollider && playerRigidbody != null)
            {
                StartCoroutine(DisablePlayerGravityAndDrag());
            }

            // Apply damage logic here (if needed, add your damage application logic)
        }

        Destroy(gameObject);
    }

    IEnumerator DisablePlayerGravityAndDrag()
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.useGravity = false;
            float originalDrag = playerRigidbody.drag;
            playerRigidbody.drag = 10f; // Set to a high value to simulate zero gravity

            yield return new WaitForSeconds(disableGravityDuration);

            playerRigidbody.useGravity = true;
            playerRigidbody.drag = originalDrag;
        }
    }
}
