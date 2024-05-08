using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAplly : MonoBehaviour
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
    public AudioClip RealodSoundEffect;
    public AudioClip ErrorSoundEffect;

    [Header("Pitch Edit")]
    public float SprintingSoundEffectPitch;
    public float WalkingSoundEffectPitch;
    public float WallRunningSoundEffectPitch;
    public float WallRunningSpeedingSoundEffectPitch;
    private float SlidingSpeedingSoundEffectPitch;
    public float NormalSlidingSpeedingSoundEffectPitch;


    [Header("Objects Refrences")]
    public GameObject Player;
    public GameObject Pestol;
    public GameObject Sword;

    [Header("Script  Refrences")]
    private PlayerMovement PlayerMovement;
    private gunShot gunShot;
    private PlayerDamaging Swing;

    [Header("Volume")]
    public float bossMusicVolume;
    public float WalkingVolume;
    public float DashingVolume;
    public float SprintingVolume;
    public float SwingVolume;
    public float GunShotVolume;

    private float PlayerMovementNormalVolume;
    private float WeaponsNormalVolume;
    private float SpeedingNormalVolume;
    void Start()
    {
        PlayBackGroundMusic(bossMusic);
        PlayerMovement = Player.GetComponent<PlayerMovement>();
        gunShot = Pestol.GetComponent<gunShot>();
        Swing = Sword.GetComponent<PlayerDamaging>();

        PlayerMovementNormalVolume = PlayerMovementEffectsAduioSource.volume;
        WeaponsNormalVolume = WeaponsEffectsAduioSource.volume;
        SpeedingNormalVolume = SpeedingEffectsAduioSource.volume;

        SlidingSpeedingSoundEffectPitch = NormalSlidingSpeedingSoundEffectPitch;
    }
    private void Update()
    {
        PlayPlayerMovementSoundEffect();
        WeaponsSoundEffects();
        SpeedingSoundEffects();

    }

    public void PlayBackGroundMusic(AudioClip clip)
    {
        BackGroundAduioSource.clip = clip;
        BackGroundAduioSource.Play();
    }

    public void PlayPlayerMovementSoundEffect()
    {
        if (PlayerMovement.State == PlayerMovement.MovementState.Walking && (PlayerMovement.Horizontal != 0 || PlayerMovement.Vertical != 0) && !PlayerMovement.OnSlope())
        {
            PlayerMovementEffectsAduioSource.pitch = WalkingSoundEffectPitch;
            if (!PlayerMovementEffectsAduioSource.isPlaying)
                PlayerMovementEffectsAduioSource.PlayOneShot(WalkingSoundEffect, 0.2f);

        }
        else if (PlayerMovement.State == PlayerMovement.MovementState.Sprinting && (PlayerMovement.Horizontal != 0 || PlayerMovement.Vertical != 0) && !PlayerMovement.OnSlope())
        {
            PlayerMovementEffectsAduioSource.pitch = SprintingSoundEffectPitch;
            if (!PlayerMovementEffectsAduioSource.isPlaying)
                PlayerMovementEffectsAduioSource.PlayOneShot(WalkingSoundEffect, 0.2f);
        }
        else if (PlayerMovement.WallRunning)
        {
            PlayerMovementEffectsAduioSource.pitch = WallRunningSoundEffectPitch;
            if (!PlayerMovementEffectsAduioSource.isPlaying)
                PlayerMovementEffectsAduioSource.PlayOneShot(WalkingSoundEffect, 0.2f);
        }
        else
        {
            PlayerMovementEffectsAduioSource.Stop();
        }
    }

    public void WeaponsSoundEffects()
    {
        if (gunShot.shooting)
        {
            WeaponsEffectsAduioSource.PlayOneShot(GunShotSoundEffect, 0.3f);
        }
        else if (Swing.Swinging)
        {
            if (!WeaponsEffectsAduioSource.isPlaying)
                WeaponsEffectsAduioSource.PlayOneShot(SwingSoundEffect, 0.5f);
        }
        else if (gunShot.isReloading)
        {
            if (!WeaponsEffectsAduioSource.isPlaying)
                WeaponsEffectsAduioSource.PlayOneShot(RealodSoundEffect, 0.5f);
        }
        else if (gunShot.ErrorFullMag)
        {
            WeaponsEffectsAduioSource.PlayOneShot(ErrorSoundEffect, 0.3f);
        }
    }

    public void SpeedingSoundEffects()
    {
        if ((PlayerMovement.sliding && PlayerMovement.OnSlope() && PlayerMovement.MoveSpeed >= 15f))
        {
            SlidingSpeedingSoundEffectPitch += Time.deltaTime * 0.1f;

            if(SlidingSpeedingSoundEffectPitch >= 1.5f)
            {
                SlidingSpeedingSoundEffectPitch = 1.5f;
            }

            SpeedingEffectsAduioSource.pitch = SlidingSpeedingSoundEffectPitch;

            if (!SpeedingEffectsAduioSource.isPlaying)
                SpeedingEffectsAduioSource.PlayOneShot(SpeedingSoundEffect, 0.4f);
        }
        else if (PlayerMovement.WallRunning)
        {
            SpeedingEffectsAduioSource.pitch = WallRunningSpeedingSoundEffectPitch;
            if (!SpeedingEffectsAduioSource.isPlaying)
                SpeedingEffectsAduioSource.PlayOneShot(SpeedingSoundEffect, 0.2f);
        }
        else if (PlayerMovement.Dashing)
        {
            SpeedingEffectsAduioSource.loop = false;
            if (!SpeedingEffectsAduioSource.isPlaying)
                SpeedingEffectsAduioSource.PlayOneShot(DashingSoundEffect, 0.5f);
        }
        else
        {
            SpeedingEffectsAduioSource.Stop();
            SlidingSpeedingSoundEffectPitch = NormalSlidingSpeedingSoundEffectPitch;
            SpeedingEffectsAduioSource.loop = false;
        }

    }
}
