using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwapGun : MonoBehaviour
{
    [Header("slots")]
    public GameObject Gun;
    public GameObject Sowrd;

    [Header("state")]
    public bool InGunSlot;
    public bool InSowrdSlot;

    [Header("Refrences")]
    public GameObject KeyBindMenu;
    private KeybindManager KeybindManager;

    // Start is called before the first frame update
    void Start()
    {
        InSowrdSlot = true;
        InGunSlot = false;

        KeybindManager = KeyBindMenu.GetComponent<KeybindManager>();
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

        if (Input.GetKeyDown(KeybindManager.GetKeyCode("GunSlot")))
        {
            InGunSlot = true;
            InSowrdSlot = false;
        }

        if (Input.GetKeyDown(KeybindManager.GetKeyCode("SwordSlots")))
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
