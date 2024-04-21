using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameArrow : MonoBehaviour
{
    public GameObject player;
    private float cd = 4;
    Vector3 playerPos;
    public Rigidbody rb;
    public int speed = 6;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Fall>().player;
        
        playerPos = player.transform.position;
        transform.LookAt(playerPos);

        
    }

    // Update is called once per frame
    void Update()
    {
        int x = 0, y = 0, z = 0;
        if(transform.position.x < playerPos.x)
        {
            x = 1;
        }
        else
        {
            x = -1;
        }
        if (transform.position.y < playerPos.y)
        {
            y = 1;
        }
        else
        {
            y = -1;
        }
        if (transform.position.z < playerPos.z)
        {
            z = 1;
        }
        else
        {
            z = -1;
        }
        Vector3 dir = new Vector3(x, y, z);
        rb.AddForce(dir * speed / 100,ForceMode.VelocityChange);


        if(cd < 0)
        {
            Destroy(gameObject);

        }
        else
        {
            cd = cd - Time.deltaTime;
        }
        if(transform.position.x > playerPos.x && transform.position.z > playerPos.z)
        {
            Destroy(gameObject) ;
        }
    }
}
