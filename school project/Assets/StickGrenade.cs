using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickGrenade : MonoBehaviour
{
    private Rigidbody rb;

    private bool targethit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(targethit)
            return;
        else
            targethit = true;

        rb.isKinematic = true;

        transform.SetParent(collision.transform);  
    }

}
