using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SwapGun : MonoBehaviour
{
    [Header("Slots")]
    public GameObject Pistol;
    public GameObject Sword;
    public GameObject ShotGun;
    public GameObject ARGun;
    public GameObject GrenadeLauncher;
    public GameObject Grappler;

    [Header("State")]
    private GameObject currentWeapon;
    private GameObject currentCrosshair;

    [Header("References")]
    public GameObject KeyBindMenu;
    private KeybindManager KeybindManager;

    private PlayerMovement playerMovement;

    [Header("Crosshair")]
    public GameObject PistolCrosshair;
    public GameObject ARCrosshair;
    public GameObject GrenadelauncherCrosshair;
    public GameObject shotgunCrosshair;


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        KeybindManager = KeyBindMenu.GetComponent<KeybindManager>();

        EquipWeapon(Pistol);
        ChangeCrosshair(PistolCrosshair);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeybindManager.GetKeyCode("GunSlot")))
        {
            EquipWeapon(Pistol);
            ChangeCrosshair(PistolCrosshair);
        }

        if (Input.GetKeyDown(KeybindManager.GetKeyCode("SwordSlot")))
        {
            EquipWeapon(Sword);
        }

        if (Input.GetKeyDown(KeybindManager.GetKeyCode("ShotGunSlot")))
        {
            EquipWeapon(ShotGun);
            ChangeCrosshair(shotgunCrosshair);
        }

        if (Input.GetKeyDown(KeybindManager.GetKeyCode("ARGunSlot")))
        {
            EquipWeapon(ARGun);
            ChangeCrosshair(ARCrosshair);
        }

        if (Input.GetKeyDown(KeybindManager.GetKeyCode("GrapplerSlot")))
        {
            EquipWeapon(Grappler);
            ChangeCrosshair(Grappler);
        }

    }

    private void EquipWeapon(GameObject weapon)
    {
        if (currentWeapon != null)
        {

            Weapon currentWeaponScript = currentWeapon.GetComponent<Weapon>();
            if (currentWeaponScript != null)
            {
                currentWeaponScript.StopReload();
            }

            currentWeapon.SetActive(false);
        }

        currentWeapon = weapon;
        currentWeapon.SetActive(true);

        ApplyWeaponAbilities();
    }

    private void ChangeCrosshair(GameObject Crosshair)
    {
        if (currentCrosshair != null)
        {
            currentCrosshair.SetActive(false);
        }

        currentCrosshair = Crosshair;
        currentCrosshair.SetActive(true);
    }

    private void ApplyWeaponAbilities()
    {
        if (currentWeapon == Pistol)
        {
            playerMovement.walkSpeed *= 1.2f;
            playerMovement.sprintSpeed *= 1.2f;
            playerMovement.wallRunSpeed *= 1.2f;
        }
        else
        {
            playerMovement.walkSpeed = playerMovement.defaultWalkSpeed;
            playerMovement.sprintSpeed = playerMovement.defaultSprintSpeed;
            playerMovement.wallRunSpeed = playerMovement.defaultWallRunSpeed;
        }
    }
}
