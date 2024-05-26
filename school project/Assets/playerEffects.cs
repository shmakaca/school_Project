using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerEffects : MonoBehaviour
{

    public PlayerMovement PlayerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "EB")
        {
           //make a state thats called "shocked" that happens when you are hit by electirBall and call it from here
        }
        if(other.gameObject.tag == "FB")
        {
            FindAnyObjectByType<deathManager>().ifDead = true;
        }
        
        //snowBall only does nockback
    }
}
