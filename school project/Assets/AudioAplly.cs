using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAplly : MonoBehaviour
{
    [Header(" Aduio Source Refrences")]
    public AudioSource BackGroundAduioSource;
    public AudioSource PlayerMovementAduioSource;
    public AudioSource WeaponsAduioSource;
    public AudioSource SpeedingAduioSource;
    public AudioSource ParticlesAudioSource;

    [Header("Aduio Clips  Refrences")]
    public AudioClip bossMusic;
    public AudioClip WalkingSoundEffect;
    public AudioClip DashingSoundEffect;
    public AudioClip SpeedingSoundEffect;
    public AudioClip GunShotSoundEffect;
    public AudioClip SwingSoundEffect;
    public AudioClip RealodSoundEffect;
    public AudioClip ErrorSoundEffect;
    public AudioClip SlidingOnGroundSoundEffect;
    public AudioClip SloMoSoundEffect;

    [Header("Pitch Controll")]
    public  float SprintingPitch;
    public float WalkingPitch;
    public float WallRunningPitch;
    public float WallRunningSpeedingPitch;
    public float CrouchingPitch;
    private float SlidingSpeedingPitch;
    public  float NormalSlidingSpeedingPitch;

    [Header("Objects Refrences")]
    public GameObject Player;
    public GameObject Pestol;
    public GameObject Sword;

    [Header("Script  Refrences")]
    private PlayerMovement PlayerMovement;
    private gunShot gunShot;
    private PlayerDamaging Swing;
    private static TimeMange TimeMange;

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
        BackGroundMusic(bossMusic);

        TimeMange = Player.GetComponent<TimeMange>();
        PlayerMovement = Player.GetComponent<PlayerMovement>();
        gunShot = Pestol.GetComponent<gunShot>();
        Swing = Sword.GetComponent<PlayerDamaging>();

        PlayerMovementNormalVolume = PlayerMovementAduioSource.volume;
        WeaponsNormalVolume = WeaponsAduioSource.volume;
        SpeedingNormalVolume = SpeedingAduioSource.volume;

        SlidingSpeedingPitch = NormalSlidingSpeedingPitch;
    }
    private void Update()
    {
        PlayerMovementSoundEffect();
        WeaponsSoundEffects();
        SpeedingSoundEffects();
        ParticleSoundEffect();
    }

    public void BackGroundMusic(AudioClip clip)
    {
        BackGroundAduioSource.clip = clip;
        BackGroundAduioSource.Play();
    }

    public void PlayerMovementSoundEffect()
    {
        if (PlayerMovement.State == PlayerMovement.MovementState.Walking && (PlayerMovement.Horizontal != 0 || PlayerMovement.Vertical != 0) && !PlayerMovement.OnSlope())
        {
            PlayerMovementAduioSource.pitch = WalkingPitch;

            if (!PlayerMovementAduioSource.isPlaying)
                PlayerMovementAduioSource.PlayOneShot(WalkingSoundEffect, 0.1f);
        }

        else if (PlayerMovement.State == PlayerMovement.MovementState.Sprinting && (PlayerMovement.Horizontal != 0 || PlayerMovement.Vertical != 0) && !PlayerMovement.OnSlope())
        {
            PlayerMovementAduioSource.pitch = SprintingPitch;

            if (!PlayerMovementAduioSource.isPlaying)
                PlayerMovementAduioSource.PlayOneShot(WalkingSoundEffect, 0.1f);
        }

        else if (PlayerMovement.State == PlayerMovement.MovementState.Crouching && (PlayerMovement.Horizontal != 0 || PlayerMovement.Vertical != 0) && !PlayerMovement.OnSlope())
        {
            PlayerMovementAduioSource.pitch = CrouchingPitch;

            if (!PlayerMovementAduioSource.isPlaying)
                PlayerMovementAduioSource.PlayOneShot(WalkingSoundEffect, 0.1f);
        }

        else if (PlayerMovement.WallRunning)
        {
            PlayerMovementAduioSource.pitch = WallRunningPitch;

            if (!PlayerMovementAduioSource.isPlaying)
                PlayerMovementAduioSource.PlayOneShot(WalkingSoundEffect, 0.1f);
        }

        else if(PlayerMovement.sliding && !PlayerMovement.OnSlope())
        {
            if (!PlayerMovementAduioSource.isPlaying)
                PlayerMovementAduioSource.PlayOneShot(SlidingOnGroundSoundEffect, 0.3f);
        }
        else
        {
            PlayerMovementAduioSource.Stop();
            PlayerMovementAduioSource.pitch = 1f;
        }
    }

    public void WeaponsSoundEffects()
    {
        if (gunShot.IsShooting)
        {
                WeaponsAduioSource.PlayOneShot(GunShotSoundEffect, 0.15f);
        }

        else if (Swing.Swinging)
        {
            if (!WeaponsAduioSource.isPlaying)
                WeaponsAduioSource.PlayOneShot(SwingSoundEffect, 0.2f);
        } 
        else if (gunShot.isReloading)
        {
            if (!WeaponsAduioSource.isPlaying)
                WeaponsAduioSource.PlayOneShot(RealodSoundEffect, 0.15f);
        }
        else if (gunShot.ErrorFullMag)
        {
            if (!WeaponsAduioSource.isPlaying)
                WeaponsAduioSource.PlayOneShot(ErrorSoundEffect, 0.3f);
        }
    }

    public void SpeedingSoundEffects()
    {
        if ((PlayerMovement.sliding && PlayerMovement.OnSlope() && PlayerMovement.MoveSpeed >= 15f))
        {
            SlidingSpeedingPitch += Time.deltaTime * 0.1f;

            SpeedingAduioSource.pitch = SlidingSpeedingPitch;

            if (!SpeedingAduioSource.isPlaying)
                SpeedingAduioSource.PlayOneShot(SpeedingSoundEffect, 0.4f);
        }
        else if (PlayerMovement.WallRunning)
        {
            SpeedingAduioSource.pitch = WallRunningSpeedingPitch;
            if (!SpeedingAduioSource.isPlaying)
                SpeedingAduioSource.PlayOneShot(SpeedingSoundEffect, 0.1f);
        }
        else if (PlayerMovement.Dashing)
        {
            SpeedingAduioSource.loop = false;

            if (!SpeedingAduioSource.isPlaying)
                SpeedingAduioSource.PlayOneShot(DashingSoundEffect, 0.5f);
        }
        else
        {
            SpeedingAduioSource.Stop();
            SlidingSpeedingPitch = NormalSlidingSpeedingPitch;
            SpeedingAduioSource.loop = true;
        }

    }

    public void ParticleSoundEffect()
    {
        if (Input.GetMouseButtonDown((int)TimeMange.SLowMotionKey))
        {
            TimeMangePichChange();

            ParticlesAudioSource.PlayOneShot(SloMoSoundEffect, 0.1f);
        }
    }

    public void TimeMangePichChange()
    {
        SprintingPitch *= TimeMange.SlowDownFactor;
        WalkingPitch *= TimeMange.SlowDownFactor;
        WallRunningPitch *= TimeMange.SlowDownFactor;
        WallRunningSpeedingPitch *= TimeMange.SlowDownFactor;
        CrouchingPitch *= TimeMange.SlowDownFactor;
        SlidingSpeedingPitch *= TimeMange.SlowDownFactor;
        NormalSlidingSpeedingPitch *= TimeMange.SlowDownFactor;
    }
}
