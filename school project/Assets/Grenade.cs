using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float speed = 15f;
    public float explosionForce = 700f;
    public float explosionRadius = 5f;
    public float lifetime = 5f;
    public GrenadeLauncher Launcher { get; set; }

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * speed;
            Destroy(gameObject, lifetime);
        }
        else
        {
            Debug.LogError("Rigidbody component is missing on the grenade!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        if (Launcher != null && Launcher.ImpactHitEffect != null)
        {
            GameObject explosionEffect = Instantiate(Launcher.ImpactHitEffect, transform.position, Quaternion.identity);
            Destroy(explosionEffect, 3f);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
        Destroy(gameObject);
    }
}
