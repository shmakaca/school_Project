using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class fireCircle : MonoBehaviour
{
    public Transform player;
    public float radius;
    public GameObject warning;
    public ParticleSystem fcParticles;
    public bool attacking = false;
    private bool haveExit = false;
    private bool haveEnter = false;
    float cooldown = 2;
    public float staticCD ;
    public float warningTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (haveExit)
        {
            haveEnter = false ;
            haveExit = false ;
        }

        if(haveEnter)
        {
            

             if (cooldown > 0 )
             {
                 cooldown = cooldown - Time.deltaTime;
               
             }
             else
             {
                  attacking = true;
                  
                cooldown = staticCD;
                haveEnter = false;
             }
              
             
        }
        

        if (attacking)
        {
            warning.SetActive(true);
            
            Invoke("FireCircle", warningTime);
            
            attacking = false;
        }
    }

    void FireCircle()
    {
        //particles
        fcParticles.Play();


        //if player inside the circle player dies
        if((Mathf.Abs(player.position.x - transform.position.x) < radius && Mathf.Abs(player.position.z - transform.position.z) < radius) && Mathf.Abs(player.position.y - transform.position.y) < radius)
        {
            FindAnyObjectByType<deathManager>().ifDead = true;  

        } 
        warning?.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            haveEnter = true;
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            haveExit = true;
        }
    }

}
