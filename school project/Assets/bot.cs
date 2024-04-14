using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class bot : MonoBehaviour
{
    public GameObject player;
    public Transform playerPos;
    public Rigidbody botRB;
    public Quaternion rotBot;

    public int botSpeed = 1500;
    public float x=0 ,y=0 ,z=0 ;
    public float Range;

    public float rotatetime = 4;

    public bool xStay = false, yStay = false, zStay = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if(rotatetime < 0)
        {


            var tar = Quaternion.Euler(-90, 0, 0);
            botRB.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, tar, 1);
           
            rotatetime = 4;
        }
        else
        {
            rotatetime = rotatetime - Time.deltaTime;
        }



        playerPos = player.GetComponent<Transform>();

        if (Mathf.Abs(transform.position.x - playerPos.position.x) < Range)
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

        Vector3 botD = new Vector3(x, y, z).normalized;

        botRB.AddForce(botD * botSpeed *Time.deltaTime ,ForceMode.Force);

    }
}
