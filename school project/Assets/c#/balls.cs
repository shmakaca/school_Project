using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balls : MonoBehaviour
{

    private Transform playerRN;
    private Rigidbody rb;
    private float speed = 200f;

    // Start is called before the first frame update
    void Start()
    {
        this.playerRN = FindAnyObjectByType<PlayerMovement>().transform;
        this.rb = GetComponent<Rigidbody>(); 
        transform.LookAt(this.playerRN.position);
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
       

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Enemy" )
        {
            Destroy(this.gameObject);
        }
        
    }
}
