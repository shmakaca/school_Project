using System.Collections;
using System.Collections.Generic;
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
    private bool isPistolEquipped = false;

    [Header("References")]
    public GameObject KeyBindMenu;
    public GameObject player;
    private KeybindManager KeybindManager;
    private Animator parentAnimator;

    private PlayerMovement playerMovement;

    [Header("Crosshair")]
    public GameObject PistolCrosshair;
    public GameObject ARCrosshair;
    public GameObject GrenadelauncherCrosshair;
    public GameObject shotgunCrosshair;

    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        KeybindManager = KeyBindMenu.GetComponent<KeybindManager>();
        parentAnimator = GetComponentInParent<Animator>(); // Get the Animator from the parent

        EquipWeapon(Pistol);
        ChangeCrosshair(PistolCrosshair);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeybindManager.GetKeyCode("GunSlot")))
        {
            if (currentWeapon != Pistol)
            {
                EquipWeapon(Pistol);
                ChangeCrosshair(PistolCrosshair);
            }
        }

        if (Input.GetKeyDown(KeybindManager.GetKeyCode("SwordSlot")))
        {
            if (currentWeapon != Sword)
            {
                EquipWeapon(Sword);
            }
        }

        if (Input.GetKeyDown(KeybindManager.GetKeyCode("ShotGunSlot")))
        {
            if (currentWeapon != ShotGun)
            {
                EquipWeapon(ShotGun);
                ChangeCrosshair(shotgunCrosshair);
            }
        }

        if (Input.GetKeyDown(KeybindManager.GetKeyCode("ARGunSlot")))
        {
            if (currentWeapon != ARGun)
            {
                EquipWeapon(ARGun);
                ChangeCrosshair(ARCrosshair);
            }
        }

        if (Input.GetKeyDown(KeybindManager.GetKeyCode("GrapplerSlot")))
        {
            if (currentWeapon != Grappler)
            {
                EquipWeapon(Grappler);
                ChangeCrosshair(Grappler);
            }
        }
    }

    private void EquipWeapon(GameObject weapon)
    {
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);
        }

        if (currentWeapon != weapon)
        {
            // Trigger the swap animation
            if (parentAnimator != null)
            {
                parentAnimator.SetTrigger("Swap");
            }
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
            if (!isPistolEquipped)
            {
                playerMovement.walkSpeed *= 1.2f;
                playerMovement.sprintSpeed *= 1.2f;
                playerMovement.wallRunSpeed *= 1.2f;
                isPistolEquipped = true;
            }
        }
        else
        {
            playerMovement.walkSpeed = playerMovement.defaultWalkSpeed;
            playerMovement.sprintSpeed = playerMovement.defaultSprintSpeed;
            playerMovement.wallRunSpeed = playerMovement.defaultWallRunSpeed;
            isPistolEquipped = false;
        }
    }
}
