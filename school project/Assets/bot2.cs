using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bot2 : MonoBehaviour
{
    [Header("-----------")]
    public Transform player;
    public Rigidbody rb;

    [Header("-----------")]
    public float speed;
    public float range;

    [Header("-----------")]
    public float x = 0 , y = 0 , z = 0 ;
    [Header("-----------")]
    public bool xStay = false, yStay = false, zStay = false;

   private float time;
    public float kTime;


    public Vector3 outRange = new Vector3(50, 50, 50);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if bot in range on each axis x,y,z
        if (Mathf.Abs(transform.position.x - player.position.x )<= range)
        {
            xStay = true;
        }
        else
        {
            xStay = false;
        }

        if (Mathf.Abs(transform.position.y - player.position.y )<= range)
        {
            yStay = true;
        }
        else
        {
            yStay = false;
        }

        if (Mathf.Abs(transform.position.z - player.position.z )<= range)
        {
            zStay = true;
        }
        else
        {
            zStay = false;
        }

        //-----------------------------

        if (transform.position.x < player.position.x && !xStay)
        {
            x = 1;

        }
        else if (!xStay)
        {
            x = -1;
        }
        else
        {
            x = 0;
        }

        if (transform.position.y < player.position.y && !yStay)
        {
            y = 1;

        }
        else if(!yStay)
        {
            y = -1;
        }
        else
        {
            y = 0;
        }

        if (transform.position.z < player.position.z && !zStay)
        {
            z = 1;

        }
        else if (!zStay)
        {
            z = -1;
        }
        else
        {
            z = 0;
        }


        Vector3 dir = new Vector3(x, y, z);
        rb.AddForce(dir * speed , ForceMode.VelocityChange);




        //rotation



        if (time < 0)
        {
            
            transform.LookAt(player.position);
            rb.angularVelocity = Vector3.zero;
            time = kTime;
        }
        else
        {
            
            time = time - Time.deltaTime;
        }
        
        if ((transform.position.x - player.position.x) > outRange.x)
        {
            transform.position = player.position;

        }


        if ((transform.position.y - player.position.y) > outRange.y)
        {
            transform.position = player.position;

        }


        if ((transform.position.z - player.position.z) > outRange.z)
        {
            transform.position = player.position;

        }










    }
}
