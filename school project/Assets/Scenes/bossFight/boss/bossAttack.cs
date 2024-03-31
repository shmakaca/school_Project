using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAttack : MonoBehaviour
{
    public Object starFall;
    private float cdStarFall = 10;
    public bool isStarFall = false;








    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cdStarFall > 0) 
        {
            isStarFall = false;
            cdStarFall-= Time.deltaTime;
        }
        else
        {
            isStarFall = true;
            cdStarFall = 10;
            Debug.Log("gay");
        }
        






    }
}
