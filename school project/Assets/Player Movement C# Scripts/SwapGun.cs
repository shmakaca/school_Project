using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapGun : MonoBehaviour
{
    [Header("slots")]
    public GameObject Gun;
    public GameObject Sowrd;

    [Header("swap")]
    public KeyCode swapKey;

    [Header("state")]
    public bool Guning;
    public bool Sowrding;


    // Start is called before the first frame update
    void Start()
    {
        Sowrding = true;
        Guning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Sowrding)
        {
            so();
        }
        if(Guning)
        {
            gu();
        }

        if(Input.GetKeyDown(swapKey))
        {
            if(Sowrding)
            {
                Sowrding = false;
            }
            else
            {
                Sowrding = true;
            }

            if (Guning)
            {
                Guning = false;
            }
            else
            {
                Guning = true;
            }
        }
        
    }
    private void so()
    {
        Sowrd.SetActive(true);
        Gun.SetActive(false);
    }
    private void gu()
    {
        Sowrd.SetActive(false);
        Gun.SetActive(true);
    }
}
