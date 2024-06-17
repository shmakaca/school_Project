using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;
    public float lifetime = 3f;
    public GameObject impactEffectPrefab;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Apply damage logic here
        Debug.Log(collision.transform.name + " hit by Shotgun Bullet");
        if (impactEffectPrefab != null)
        {
            GameObject impactEffect = Instantiate(impactEffectPrefab, transform.position, Quaternion.LookRotation(collision.contacts[0].normal));
            Destroy(impactEffect, 2f); // Destroy impact effect after 2 seconds
        }
        Destroy(gameObject);
    }
}
