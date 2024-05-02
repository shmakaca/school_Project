using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beaconActivation : MonoBehaviour
{
    public GameObject beacon;
    public bool ifActive = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ifActive = FindAnyObjectByType<Npctalk>().haveTalked;

        if (ifActive)
        {
            beacon.SetActive(true);
        }
        else
        {
            beacon.SetActive(false);
        }


    }
}
