using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShot : MonoBehaviour
{
    [Header("References")]
    public Transform Player;
    public Transform PlayerCam;
    private Rigidbody rb;
    public GameObject AudioManger;
    private PlayerMovement playerMovement;
    private AudioApply audioApply;
    public GameObject KeyBindMenu;
    private KeybindManager keybindManager;
    private SwapGun swapGun;
    private WeaponSway weaponSway; // Local WeaponSway reference
    private AudioSource audioSource;




}

