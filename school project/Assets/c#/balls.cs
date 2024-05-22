using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balls : MonoBehaviour
{

    private Transform playerRN;
    private Rigidbody rb;
    private float speed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        this.playerRN = FindAnyObjectByType<PlayerMovement>().transform;
        this.rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(this.playerRN.position);

        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            Destroy(this.gameObject);
        }
        
    }
}
