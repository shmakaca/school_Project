using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesWall : MonoBehaviour
{
    private float wallDesCD = 3;

    // Update is called once per frame
    void Update()
    {
        if (wallDesCD > 0)
        {
            wallDesCD = wallDesCD - Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }



    }
}
