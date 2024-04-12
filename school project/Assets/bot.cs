using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bot : MonoBehaviour
{
    public GameObject player;
    public Transform playerPos;
    public Rigidbody botRB;

    public int botSpeed = 50;
    public float x=0 ,y=0 ,z=0 ;
    public float Range;

    public bool xStay = false, yStay = false, zStay = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        



        playerPos = player.GetComponent<Transform>();

        if (Mathf.Abs( transform.position.x - playerPos.position.x ) < Range)
        {
            x = 0;

            xStay = true;
        }
        else
        {
            xStay = false ;
        }
        if (Mathf.Abs(transform.position.y - playerPos.position.y) < Range)
        {
            y = 0;
            yStay = true;
        }
        else
        {
            yStay = false;
        }
        if (Mathf.Abs(transform.position.z - playerPos.position.z) < Range)
        {
            z = 0;
            zStay = true;
        }
        else
        {
            zStay = false;
        }




        if ( transform.position.x < playerPos.position.x && !xStay)
        {
            x = 1;

        }
        else if(!xStay)
        {
            x = -1;
        }

        if(transform.position.z < playerPos.position.z && !zStay)
        {
            z = 1;

        }
        else if(!zStay)
        {
            z = -1;
        }

        if(transform.position.y < playerPos.position.y && !yStay)
        {

             y = 1;
        }
        else if(!yStay)
        {
            y = -1;
        }

        Vector3 botD = new Vector3(x, y, z);

        botRB.AddForce(botD * botSpeed *Time.deltaTime ,ForceMode.Force);

    }
}
