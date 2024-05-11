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
    private AudioAplly Am;

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

    [Header("Camera Effects")]
    public playercamera Cam;
    public float KnockBackFovChange;

    void Start()
    {
        isReloading = false;
        ReadyToShootAgain = true;

        shotsNum = mag;
        ShotsUsed = 0;

        PlayerMovement = Player.GetComponent<PlayerMovement>();
        rb = Player.GetComponent<Rigidbody>();
        Am = AudioManger.GetComponent<AudioAplly>();
    }

    // Update is called once per frame
    void Update()
    {
        isGun = FindAnyObjectByType<SwapGun>().InGunSlot;
        if (isGun && Input.GetKeyDown(KeyCode.Mouse0) && !isReloading && shotsNum > 0 && ReadyToShootAgain)
        {
            IsShooting = true;
            shot();
        }
        else
        {
            IsShooting = false;
        }

        if (isGun && Input.GetKeyDown(KeyCode.R) && !IsfullMag())
        {
            Reloading();
        }

        if (isGun && Input.GetKeyDown(KeyCode.R) && IsfullMag())
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

        Cam.DOFOV(KnockBackFovChange + PlayerMovement.NormalPov);
    }

    private void StopKnockBack()
    {
        PlayerMovement.Shooting = false;

        Cam.DOFOV(PlayerMovement.NormalPov);

    }
    public void Reloading()
    {
        isReloading = true;
        shotsNum = mag;
        animator.SetTrigger("trReload");

        Invoke(nameof(StopRelaoding), Am.RealodSoundEffect.length * ShotsUsed);

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
