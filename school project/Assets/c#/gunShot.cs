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
    private PlayerMovement PlayerMovement;
    public AudioSource ShootSound;

    public int bullDamage;

    private bool isGun;
    private bool isReloading;
    private int shotsNum;
    private int mag = 3;

    [Header("KnockBack")]
    public float KnockBackForce;
    public float KnockBackDuration;

    [Header("Camera Effects")]
    public playercamera Cam;
    public float KnockBackFov;

    void Start()
    {
        isReloading = false;
        shotsNum = mag;
        PlayerMovement = Player.GetComponent<PlayerMovement>();
        rb = Player.GetComponent<Rigidbody>();
        ShootSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        isGun = FindAnyObjectByType<SwapGun>().Guning;
        if(isGun && Input.GetKeyDown(KeyCode.Mouse0) && !isReloading && shotsNum > 0)
        {
            ShootSound.Play();
            shot();
            shotsNum--;
        }

        if (isGun && Input.GetKeyDown(KeyCode.R) )
        {
            isReloading = true;
            animator.SetTrigger("trReload");
            Invoke("Reloading", 2f);
            isReloading =false;
        }
        
    }
    private void shot()
    {
        bool koko = true;
        if(koko)
        {
            Instantiate(Bullet, bulletHall);
            koko = false;
            KnockBAck();
        }
    }

    private void KnockBAck()
    {
        PlayerMovement.Shooting = true;

        Vector3 direction = -PlayerCam.transform.forward;

        Vector3 ForceToApply = direction * KnockBackForce;

        rb.AddForce(ForceToApply, ForceMode.Impulse);

        Invoke(nameof(StopKnockBack), KnockBackDuration);

        Cam.DOFOV(KnockBackFov);
    }

    private void StopKnockBack()
    {
        PlayerMovement.Shooting = false;

        Cam.DOFOV(80f);

    }
    private void Reloading()
    {    
        shotsNum = mag;
    }

}
