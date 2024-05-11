using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwapGun : MonoBehaviour
{
    [Header("slots")]
    public GameObject Gun;
    public GameObject Sowrd;

    [Header("swap")]
    public MouseButton SwordSlotkey;
    public KeyCode GunSlotKey;

    [Header("state")]
    public bool InGunSlot;
    public bool InSowrdSlot;


    // Start is called before the first frame update
    void Start()
    {
        InSowrdSlot = true;
        InGunSlot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (InSowrdSlot)
        {
            sowrd();
        }
        if (InGunSlot)
        {
            gun();
        }

        if (Input.GetMouseButtonDown(4))
        {
            InGunSlot = true;
            InSowrdSlot = false;
        }

        if (Input.GetMouseButtonDown(3))
        {
            InGunSlot = false ;
            InSowrdSlot = true;
        }

    }
    private void sowrd()
    {
        Sowrd.SetActive(true);
        Gun.SetActive(false);
    }
    private void gun()
    {
        Sowrd.SetActive(false);
        Gun.SetActive(true);
    }
}
