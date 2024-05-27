using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    private Rigidbody rb;
    public float DeafultBulletSpeed;
    private float BulletSpeed;
    private int damage;
    private Transform Oreientation;
    bool InSlowMotion;
    float SlowDownFactor;
    // Start is called before the first frame update
    void Start()
    {
        damage = FindAnyObjectByType<gunShot>().bullDamage;
        Oreientation = FindAnyObjectByType<PlayerMovement>().Orientation;
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(Oreientation.forward * DeafultBulletSpeed, ForceMode.Impulse);
        InSlowMotion = FindAnyObjectByType<TimeMange>().InSlowMotion;
        SlowDownFactor = FindAnyObjectByType<TimeMange>().SlowDownFactor;

        BulletSpeed = DeafultBulletSpeed;
    }

    private void Update()
    {
        SpeedControl();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "boss")
        {
            FindAnyObjectByType<bossHealth>().Damage(damage);
            Destroy(gameObject);

        }
    }

    private void SpeedControl()
    {
        if(InSlowMotion)
        {
            BulletSpeed *= SlowDownFactor;
        }
    }

    private void Reset()
    {
        BulletSpeed = DeafultBulletSpeed;
    }
}
