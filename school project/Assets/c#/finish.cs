using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finish : MonoBehaviour
{
    public GameObject doors;

    public bool secColl = false;
    public bool isClose = false;
    public bool canFinish = false;
    public int mushCount = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        mushCount = FindAnyObjectByType<mushTake>().mushNum;
        canFinish = FindAnyObjectByType<Npctalk>().haveTalked;


        if (isClose)
        {
            if (canFinish && mushCount >= 5)
            {
                missionComplete();

                mushCount = mushCount - 5;

            }

        }

    }

    void missionComplete()
    {
        doors.SetActive(false);

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "secNpc")
        {
            isClose = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "secNpc")
        {
            isClose = false;
        }
    }
}
