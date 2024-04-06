using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int timeLaser;
    public GameObject imageUI;
    public float laserCD;
    public float laserCDtemp;
    private bool isntMoving;

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
        isntMoving = FindAnyObjectByType<PlayerMovement>();

        Invoke("kill", 1);
    }
    private void kill()
    {
        if (!isntMoving)
        {
            FindAnyObjectByType<respawn>().respon();
        }

    }
    
}

