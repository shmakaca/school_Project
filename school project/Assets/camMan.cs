using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camMan : MonoBehaviour
{
    public Camera mainCam;

    [Header("npc")]
    public bool isNpcDone = true;
    public bool isNpcCam = false;
    public Camera npcCam;



    // Start is called before the first frame update
    void Start()
    {
        def();
        
    }

    // Update is called once per frame
    void Update()
    {
        isNpcCam = FindAnyObjectByType<Npctalk>().isTalking;
        isNpcDone = FindAnyObjectByType<Npctalk>().isDone;
        if (isNpcCam)
        {
            
            npc();
        }
        else
        {
            
            def() ;
        }

        if(isNpcDone)
        {
            
            def();
            isNpcCam = false;
        }
        





    }
    void def()
    {
        mainCam.enabled = true;
        npcCam.enabled = false;
    }
    void npc()
    {
        npcCam.enabled = true;
        mainCam.enabled = false;
    }
}
