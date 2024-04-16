using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class table : MonoBehaviour
{
    public GameObject player;
    private bool isCloseTable = false;
    public bool tableUse = false;

    void Update()
    {
        if (isCloseTable)
        {

            if (Input.GetKeyUp(KeyCode.F))
            {
                player.GetComponent<PlayerMovement>().enabled = false;
                Debug.Log("ff");
                tableUse = true;
                isCloseTable = false;
            }

        }
        else if(tableUse)
        {
            if(Input.GetKeyUp(KeyCode.F))
            {
                player.GetComponent<PlayerMovement>().enabled = true;
                tableUse = false ;

            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isCloseTable = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isCloseTable = false;
        }
    }

}
