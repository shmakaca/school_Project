using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;


public class gunShot : MonoBehaviour
{
    public Animator animator;

    [Header("Refrences")]
    public TextMeshProUGUI bullText;
    public Transform bulletHall;
    public Transform Player;
    public Transform PlayerCam;
    private Rigidbody rb;
    public GameObject Bullet;
    public GameObject AudioManger;
    private PlayerMovement PlayerMovement;
    private AudioApply Am;
    public GameObject KeyBindMenu;
    private KeybindManager KeybindManager;

    public int bullDamage;

    private bool isGun;
    private bool ReadyToShootAgain;
    public bool isReloading;
    public bool IsShooting;
    public bool ErrorFullMag;
    private int shotsNum;
    private float ShotsUsed;
    public float FireRate;
    private int mag = 3;

    [Header("KnockBack")]
    public float KnockBackForce;
    public float KnockBackDuration;


    void Start()
    {
        isReloading = false;
        ReadyToShootAgain = true;

        shotsNum = mag;
        ShotsUsed = 0;

        PlayerMovement = Player.GetComponent<PlayerMovement>();
        rb = Player.GetComponent<Rigidbody>();
        Am = AudioManger.GetComponent<AudioApply>();
        KeybindManager = KeyBindMenu.GetComponent<KeybindManager>();

    }

    // Update is called once per frame
    void Update()
    {
        isGun = FindAnyObjectByType<SwapGun>().InGunSlot;
        if (isGun && Input.GetKeyDown(KeybindManager.GetKeyCode("Shoot")) && !isReloading && shotsNum > 0 && ReadyToShootAgain)
        {
            IsShooting = true;
            shot();
        }
        else
        {
            IsShooting = false;
        }

        if (isGun && Input.GetKeyDown(KeybindManager.GetKeyCode("Reload")) && !IsfullMag())
        {
            Reloading();
        }

        if (isGun && Input.GetKeyDown(KeybindManager.GetKeyCode("Reload")) && IsfullMag())
        {
            ErrorFullMag = true;
        }
        else
        {
            ErrorFullMag = false;
        }

    }
    private void shot()
    {
        shotsNum--;
        ShotsUsed++;

        ReadyToShootAgain = false;

        bool koko = true;
        if (koko)
        {
            Instantiate(Bullet, bulletHall);
            KnockBAck();
            koko = false;

        }
        Invoke(nameof(StopShot), FireRate);
    }

    private void StopShot()
    {
        ReadyToShootAgain = true;
    }

    private void KnockBAck()
    {
        PlayerMovement.Shooting = true;

        Vector3 direction = -PlayerCam.transform.forward;

        Vector3 ForceToApply = direction * KnockBackForce;

        rb.AddForce(ForceToApply, ForceMode.Impulse);

        Invoke(nameof(StopKnockBack), KnockBackDuration);
    }

    private void StopKnockBack()
    {
        PlayerMovement.Shooting = false;

    }
    public void Reloading()
    {
        isReloading = true;
        shotsNum = mag;
        animator.SetTrigger("trReload");

        Invoke(nameof(StopRelaoding), Am.ReloadSoundEffect.length * ShotsUsed);

    }

    private bool IsfullMag()
    {
        if(shotsNum != mag)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void StopRelaoding()
    {
        isReloading = false ;
        ShotsUsed = 0;
    }
}
