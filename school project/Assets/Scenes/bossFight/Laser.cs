using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int timeLaser;
    public GameObject imageUI;
    public float laserCD;
    public float laserCDtemp;
    
    public float x;
    // Start is called before the first frame update
    void Start()
    {
        laserCD = Random.Range(15, 20);
        laserCDtemp = laserCD; 

    }
    private void Update()
    {
        if (laserCDtemp > 0)
        {
           laserCDtemp = laserCDtemp - Time.deltaTime;
        }
        else
        {
            LaserSummon();
            laserCDtemp = laserCD;
        }

    }
    private void LaserSummon()
    {
        Instantiate(imageUI, imageUI.transform);
        

        Invoke("kill", 2);
    }
    private void kill()
    {
        if ( x!= 0 )
        {
            x = FindAnyObjectByType<PlayerMovement>().MoveSpeed;
            FindAnyObjectByType<respawn>().respon();
        }

    }
    
}

