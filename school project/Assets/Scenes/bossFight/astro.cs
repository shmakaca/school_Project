using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class astro : MonoBehaviour
{
    

   
     void OnTriggerEnter(Collider co )
    {
        if (co.gameObject.tag == "player")
        {
            Destroy(this.gameObject);
        }
        if ( co.gameObject.tag == "despawner") 
        {

            Destroy(this.gameObject);
        
        
        }
    }
}
