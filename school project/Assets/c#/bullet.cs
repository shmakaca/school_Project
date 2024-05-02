using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = 100;
    private int damage;
    private Transform Oreientation;

    // Start is called before the first frame update
    void Start()
    {
        damage = FindAnyObjectByType<gunShot>().bullDamage;
        Oreientation = FindAnyObjectByType<PlayerMovement>().Oreientation;
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(Oreientation.forward * speed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "boss")
        {
            FindAnyObjectByType<bossHealth>().Damage(damage);
            Destroy(gameObject);

        }
    }
}
