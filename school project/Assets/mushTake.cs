using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mushTake : MonoBehaviour
{
    public GameObject pickUp;
    public GameObject mush = null;
    public bool canTakeMushroom = false;
    public bool desF;
    public bool readyPickup;
    public int mushNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()   
    {
        if (canTakeMushroom )//&& this.transform.forward == mush.position
        {
            pickUp.SetActive( true );
            

            if( Input.GetKey(KeyCode.F) && readyPickup) 
            {
                Destroy(mush);
                mushNum ++;
                readyPickup = false;
            }
            canTakeMushroom=false;
        }
        if(desF) 
        {
           pickUp.SetActive(false);
                
            desF=false;
        }
    }
    public void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.tag == "MUSHROOM")
        {
            canTakeMushroom = true;
            mush = other.gameObject;
            readyPickup = true;
        }
        else
        {
            desF = true;
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "MUSHROOM")
        {
            desF = true;
             readyPickup = false;
        }
    }
}
