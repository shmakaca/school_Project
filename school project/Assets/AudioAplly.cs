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
    public AudioSource JumpsAudioSource;

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
    public AudioClip EnterSloMoSoundEffect;
    public AudioClip ExitSloMoSoundEffect;
    public AudioClip JumPadSoundEffect;

    [Header("Pitch Controll")]
    private float SprintingPitch;
    private float WalkingPitch;
    private float WallRunningPitch;
    private float WallRunningSpeedingPitch;
    private float CrouchingPitch ;
    private float SlidingSpeedingPitch ;
    private float DashingPitch;
    private float ReloadPitch;
    private float GunShotPitch;
    private float JumpPadPitch;

    [Header("Pitch Reset")] 
    public float NormalSprintingPitch;
    public float NormalWalkingPitch;
    public float NormalWallRunningPitch;
    public float NormalWallRunningSpeedingPitch;
    public float NormalCrouchingPitch;
    public float NormalSlidingSpeedingPitch;
    public float NormalDashingPitch;
    public float NormalReloadPitch;
    public float NormalGunShotPitch;
    public float NormalJumpPadPitch;

    [Header("Objects Refrences")]
    public GameObject Player;
    public GameObject Pestol;
    public GameObject Sword;

    [Header("Script  Refrences")]
    private PlayerMovement PlayerMovement;
    private gunShot gunShot;
    private PlayerDamaging Swing;
    private static TimeMange TimeMange;
    private SwapGun SwapGun;

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
        SwapGun = Player.GetComponent<SwapGun>();

        PlayerMovementNormalVolume = PlayerMovementAduioSource.volume;
        WeaponsNormalVolume = WeaponsAduioSource.volume;
        SpeedingNormalVolume = SpeedingAduioSource.volume;

        SlidingSpeedingPitch = NormalSlidingSpeedingPitch;

        Reset();
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
        if (PlayerMovement.State == PlayerMovement.MovementState.Walking && (PlayerMovement.Horizontal != 0 || PlayerMovement.Vertical != 0) )
        {
            PlayerMovementAduioSource.pitch = WalkingPitch;

            if (!PlayerMovementAduioSource.isPlaying)
                PlayerMovementAduioSource.PlayOneShot(WalkingSoundEffect, 0.1f);
        }

        else if (PlayerMovement.State == PlayerMovement.MovementState.Sprinting && (PlayerMovement.Horizontal != 0 || PlayerMovement.Vertical != 0))
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
            WeaponsAduioSource.pitch = GunShotPitch;

            WeaponsAduioSource.PlayOneShot(GunShotSoundEffect, 0.15f);
        }

        else if (Swing.Swinging)
        {
            if (!WeaponsAduioSource.isPlaying)
                WeaponsAduioSource.PlayOneShot(SwingSoundEffect, 0.2f);
        } 
        else if (gunShot.isReloading)
        {
            WeaponsAduioSource.pitch = ReloadPitch;

            if (!WeaponsAduioSource.isPlaying)
                WeaponsAduioSource.PlayOneShot(RealodSoundEffect, 0.15f);
        }
        else if (gunShot.ErrorFullMag)
        {
            if (!WeaponsAduioSource.isPlaying)
                WeaponsAduioSource.PlayOneShot(ErrorSoundEffect, 0.3f);
        }
        else if (!SwapGun.InGunSlot)
        {
            if (Input.GetMouseButtonDown(4))
            {
                WeaponsAduioSource.PlayOneShot(RealodSoundEffect, 0.15f);
            }
                
        }

        else if (!SwapGun.InSowrdSlot)
        {
            if (Input.GetMouseButtonDown(3))
            {
                WeaponsAduioSource.PlayOneShot(SwingSoundEffect, 0.15f);
            }

        }
    }

    public void SpeedingSoundEffects()
    {
        if ((PlayerMovement.sliding && PlayerMovement.OnSlope()))
        {
            SlidingSpeedingPitch += Time.deltaTime * 0.1f;

            SpeedingAduioSource.pitch = SlidingSpeedingPitch;

            if (!SpeedingAduioSource.isPlaying)
                SpeedingAduioSource.PlayOneShot(SpeedingSoundEffect, 0.4f);
        }

        else if (PlayerMovement.Dashing)
        {
            SpeedingAduioSource.loop = false;
            SpeedingAduioSource.pitch = DashingPitch;

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

            if(!ParticlesAudioSource.isPlaying)
                ParticlesAudioSource.PlayOneShot(EnterSloMoSoundEffect, 0.1f);
        }
        else if (!TimeMange.WasInSlowmotion)
        {
            Reset();
        }


    }

    public void JumpsSoundEffects()
    {
        if (Input.GetKey(PlayerMovement.JumpKey) && PlayerMovement.ReadyToJump && PlayerMovement.OnGround)
        {
            JumpsAudioSource.PlayOneShot(JumPadSoundEffect, 0.3f);
        }
        else if(Input.GetKeyDown(PlayerMovement.JumpKey) && PlayerMovement.ReadyToDoubleJump)
        {
            JumpsAudioSource.PlayOneShot(JumPadSoundEffect, 0.3f);
        }
        else if(PlayerMovement.OnJumpPad)
        {
            JumpsAudioSource.PlayOneShot(JumPadSoundEffect, 0.3f);
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
        ReloadPitch *= TimeMange.SlowDownFactor;
        DashingPitch *= TimeMange.SlowDownFactor;
        GunShotPitch *= TimeMange.SlowDownFactor;
        JumpPadPitch *= TimeMange.SlowDownFactor;
    }

    public void Reset()
    {
        SprintingPitch = NormalSprintingPitch;
        WalkingPitch = NormalWalkingPitch;
        WallRunningPitch = NormalWallRunningPitch;
        WallRunningSpeedingPitch = NormalWallRunningSpeedingPitch;
        CrouchingPitch = NormalCrouchingPitch;
        SlidingSpeedingPitch = NormalSlidingSpeedingPitch;
        ReloadPitch = NormalReloadPitch;
        DashingPitch = NormalDashingPitch;
        GunShotPitch = NormalGunShotPitch;
        JumpPadPitch = NormalJumpPadPitch;
    }
}
