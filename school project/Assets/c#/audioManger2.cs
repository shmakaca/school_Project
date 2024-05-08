using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class audioManger2 : MonoBehaviour
{
    [Header(" Aduio Source Refrences")]
    public AudioSource BackGroundAduioSource;
    public AudioSource PlayerMovementEffectsAduioSource;
    public AudioSource WeaponsEffectsAduioSource;
    public AudioSource SpeedingEffectsAduioSource;

    [Header("Aduio Clips  Refrences")]
    public AudioClip bossMusic;
    public AudioClip WalkingSoundEffect;
    public AudioClip DashingSoundEffect;
    public AudioClip SpeedingSoundEffect;
    public AudioClip GunShotSoundEffect;
    public AudioClip SwingSoundEffect;
    public float SprintingSoundEffectPitch;
    public float WalkingSoundEffectPitch;

    [Header("Script  Refrences")]

    public GameObject Player;
    public GameObject Pestol;
    public GameObject Sword;

    private PlayerMovement PlayerMovement;
    private gunShot gunShot;
    private PlayerDamaging Swing;

    void Start()
    {
        PlayBackGroundMusic(bossMusic);
        PlayerMovement = Player.GetComponent<PlayerMovement>();
        gunShot = Pestol.GetComponent<gunShot>();
        Swing = Sword.GetComponent<PlayerDamaging>();

        PlayPlayerMovementSoundEffect();
        WeaponsSoundEffects();
        SpeedingSoundEffects();
    }
    private void Update()
    {

    }
    public void PlayBackGroundMusic(AudioClip clip)
    {
        BackGroundAduioSource.clip = clip;
        BackGroundAduioSource.Play();
    }

    public void PlayPlayerMovementSoundEffect()
    {
        if(PlayerMovement.State == PlayerMovement.MovementState.Walking && (PlayerMovement.Horizontal !=0 || PlayerMovement.Vertical != 0))
        {
            PlayerMovementEffectsAduioSource.pitch = WalkingSoundEffectPitch;
            PlayerMovementEffectsAduioSource.clip = WalkingSoundEffect;
            PlayerMovementEffectsAduioSource.Play();
        }
        else if(PlayerMovement.State == PlayerMovement.MovementState.Sprinting && (PlayerMovement.Horizontal != 0 || PlayerMovement.Vertical != 0))
        {
           PlayerMovementEffectsAduioSource.pitch = SprintingSoundEffectPitch;
           PlayerMovementEffectsAduioSource.clip = WalkingSoundEffect;
            PlayerMovementEffectsAduioSource.Play();
        }

        else if (PlayerMovement.Dashing)
        {
            PlayerMovementEffectsAduioSource.clip = DashingSoundEffect;
            PlayerMovementEffectsAduioSource.loop = false;
            PlayerMovementEffectsAduioSource.Play();
        }
        else
        {
            PlayerMovementEffectsAduioSource.loop = true;
        }

    }

    public void WeaponsSoundEffects()
    {
        if (gunShot.shooting)
        {
            WeaponsEffectsAduioSource.clip = GunShotSoundEffect;
        }
        else if (Swing.Swinging)
        {
            WeaponsEffectsAduioSource.clip = SwingSoundEffect;
        }

        
    }

    public void SpeedingSoundEffects()
    {
        if ((PlayerMovement.sliding && PlayerMovement.OnSlope() && PlayerMovement.MoveSpeed > 13))
        {
            SpeedingEffectsAduioSource.clip = SpeedingSoundEffect;
        }

    }
}
