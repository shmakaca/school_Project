using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camMan : MonoBehaviour
{
    private bool isDef = true;
    public Camera mainCam;

    [Header("npc")]
    public bool isNpcDone = true;
    public bool isNpcCam = false;
    public Camera npcCam;

    public bool istable = false;
    private bool isClose = false;
    public Camera tableCam;


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
        istable = FindAnyObjectByType<table>().tableUse;
        if (!isNpcCam && !istable)
        {
            isDef = true;
        }
        else
        {
            isDef = false;
        }


        if (!isDef)
        {
            if (istable)
            {
                table();
            }
            else if (isNpcCam)
            {
                npc();
            }



        }
        else
        {
            def();
        }

        
    }
        void def()
        {
            mainCam.enabled = true;
            npcCam.enabled = false;
            tableCam.enabled = false;
        }
        void npc()
        {
            npcCam.enabled = true;
            mainCam.enabled = false;
            tableCam.enabled = false;
        }
        void table()
        {
            npcCam.enabled = false;
            mainCam.enabled = false;
            tableCam.enabled = true ;
        }

}
        