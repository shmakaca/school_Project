using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Npctalk : MonoBehaviour
{
    public GameObject npcColl;
    public GameObject player;
    public GameObject talk1 , talk2 , talk3;
    public bool isTalking = false;
    
    
    public bool isDone = false;
    public bool haveTalked = false;

    bool b1 = true;
    bool b2 = false;
    bool b3 = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            if (isTalking)
            {
              player.GetComponent<PlayerMovement>().enabled = false;
              talk();
                
            }else 
            {
               b1 = true ; b2 = false ; b3 = false ;
               isDone = true;
               player.GetComponent<PlayerMovement>().enabled = true;
                
            }
             
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "npc")
        {
            isTalking = true;
            other.gameObject.tag = "secNpc";
        }
    }
    void talk()
    {
        
        isDone = false;

        if (b1)
        {
            talk1.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                talk1.SetActive(false);
                b1 = false;
                b2 = true;
                
            }
        }else if (b2)
        {

            talk2.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                talk2.SetActive(false);
                b1 = false;
                b2 = false;
                b3 = true;


            }
        
        }else if (b3)
        {

            talk3.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                talk3.SetActive(false);
                b3 = false;
                isTalking = false;
                haveTalked = true;
                
            }
        }
        

    }
    
   
    
}
