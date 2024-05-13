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
    public KeyCode SwordSlotkey;
    public KeyCode GunSlotKey;

    [Header("state")]
    public bool InGunSlot;
    public bool InSowrdSlot;

    public void GetWeaponsKeys()
    {
        SwordSlotkey = FindAnyObjectByType<KeyboardController>().Sowrdkc;
        GunSlotKey = FindAnyObjectByType<KeyboardController>().gunck;
    }
    // Start is called before the first frame update
    void Start()
    {
        InSowrdSlot = true;
        InGunSlot = false;

    }

    // Update is called once per frame
    void Update()
    {
        GetWeaponsKeys();
        if (InSowrdSlot)
        {
            sowrd();
        }
        if (InGunSlot)
        {
            gun();
        }

        if (Input.GetKeyDown(GunSlotKey))
        {
            InGunSlot = true;
            InSowrdSlot = false;
        }

        if (Input.GetKeyDown(SwordSlotkey))
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
