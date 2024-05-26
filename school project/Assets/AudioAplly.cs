using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioApply : MonoBehaviour
{
    [Header("Audio Source References")]
    public AudioSource BackGroundAudioSource;
    public AudioSource PlayerMovementAudioSource;
    public AudioSource WeaponsAudioSource;
    public AudioSource SpeedingAudioSource;
    public AudioSource ParticlesAudioSource;
    public AudioSource JumpsAudioSource;

    [Header("Audio Clips References")]
    public AudioClip bossMusic;
    public AudioClip WalkingSoundEffect;
    public AudioClip DashingSoundEffect;
    public AudioClip SpeedingSoundEffect;
    public AudioClip GunShotSoundEffect;
    public AudioClip SwingSoundEffect;
    public AudioClip ReloadSoundEffect;
    public AudioClip ErrorSoundEffect;
    public AudioClip SlidingOnGroundSoundEffect;
    public AudioClip EnterSloMoSoundEffect;
    public AudioClip ExitSloMoSoundEffect;
    public AudioClip JumpPadSoundEffect;

    [Header("Pitch")]
    private float SprintingPitch;
    private float WalkingPitch;
    private float WallRunningPitch;
    private float WallRunningSpeedingPitch;
    private float CrouchingPitch;
    private float SlidingSpeedingPitch;
    private float DashingPitch;
    private float ReloadPitch;
    private float GunShotPitch;
    private float JumpPadPitch;

    [Header("Normal Pitches")]
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

    [Header("Objects References")]
    public GameObject Player;
    public GameObject Pistol;
    public GameObject Sword;
    public GameObject MainMenu;
    public GameObject KeyBindMenu;

    [Header("Script References")]
    private PlayerMovement PlayerMovement;
    private gunShot gunShot;
    private PlayerDamaging Swing;
    private SwapGun SwapGun;
    private static mainMenu mainMenu;
    private static TimeMange TimeManage;
    private KeybindManager KeybindManager;

    [Header("Sliders References")]
    public Slider BackGroundVolumeSlider;
    public Slider ActionEffectsVolumeSlider;
    public Slider SoundEffectVolumeSlider;
    public Slider WeaponsVolumeSlider;

    [Header("Volume")]
    public float bossMusicVolume;
    public float WalkingVolume;
    public float DashingVolume;
    public float SprintingVolume;
    public float WallRunningVolume;
    public float CrouchingVolume;
    public float SlidingVolume;
    public float SwingVolume;
    public float GunShotVolume;
    public float JumpPadVolume;
    public float ReloadVolume;
    public float SpeedingVolume;
    public float ErrorVolume;
    public float SlowMotionVolume;


    [Header("Audio Settings GUI")]
    private float BackGroundVolume = 0f;
    private float ActionEffectsVolume = 0f;
    private float SoundEffectVolume = 0f;
    private float WeaponsVolume = 0f;

    void Start()
    {
        TimeManage = Player.GetComponent<TimeMange>();
        PlayerMovement = Player.GetComponent<PlayerMovement>();
        gunShot = Pistol.GetComponent<gunShot>();
        Swing = Sword.GetComponent<PlayerDamaging>();
        SwapGun = Player.GetComponent<SwapGun>();
        mainMenu = MainMenu.GetComponent<mainMenu>();
        KeybindManager = KeyBindMenu.GetComponent<KeybindManager>();

        SlidingSpeedingPitch = NormalSlidingSpeedingPitch;

        LoadSettings();
        Reset();
    }

    private void Update()
    {
        UpdateVolumeFromSliders();
        BackGroundMusic();
        ActionSoundEffect();
        WeaponsSoundEffects();
        SpeedingSoundEffects();
        ParticleSoundEffect();
        JumpsSoundEffects();
    }

    private void UpdateVolumeFromSliders()
    {
        BackGroundVolume = BackGroundVolumeSlider.value;
        ActionEffectsVolume = ActionEffectsVolumeSlider.value;
        SoundEffectVolume = SoundEffectVolumeSlider.value;
        WeaponsVolume = WeaponsVolumeSlider.value;
    }

    public void BackGroundMusic()
    {
        if (!BackGroundAudioSource.isPlaying)
        {
            BackGroundAudioSource.Play();
            BackGroundAudioSource.volume = bossMusicVolume * BackGroundVolume;
        }

        if (mainMenu.InPauseMenu)
        {
            BackGroundAudioSource.Pause();
        }
        else
        {
            BackGroundAudioSource.UnPause();
        }
    }

    public void ActionSoundEffect()
    {
        if (!mainMenu.InPauseMenu)
        {
            if (PlayerMovement.State == PlayerMovement.MovementState.Walking && (PlayerMovement.Horizontal != 0 || PlayerMovement.Vertical != 0))
            {
                PlayerMovementAudioSource.pitch = WalkingPitch;

                if (!PlayerMovementAudioSource.isPlaying)
                    PlayerMovementAudioSource.PlayOneShot(WalkingSoundEffect, WalkingVolume * ActionEffectsVolume);
            }
            else if (PlayerMovement.State == PlayerMovement.MovementState.Sprinting && (PlayerMovement.Horizontal != 0 || PlayerMovement.Vertical != 0))
            {
                PlayerMovementAudioSource.pitch = SprintingPitch;

                if (!PlayerMovementAudioSource.isPlaying)
                    PlayerMovementAudioSource.PlayOneShot(WalkingSoundEffect, SprintingVolume * ActionEffectsVolume);
            }
            else if (PlayerMovement.State == PlayerMovement.MovementState.Crouching && (PlayerMovement.Horizontal != 0 || PlayerMovement.Vertical != 0))
            {
                PlayerMovementAudioSource.pitch = CrouchingPitch;

                if (!PlayerMovementAudioSource.isPlaying)
                    PlayerMovementAudioSource.PlayOneShot(WalkingSoundEffect, CrouchingVolume * ActionEffectsVolume);
            }
            else if (PlayerMovement.WallRunning)
            {
                PlayerMovementAudioSource.pitch = WallRunningPitch;

                if (!PlayerMovementAudioSource.isPlaying)
                    PlayerMovementAudioSource.PlayOneShot(WalkingSoundEffect, WallRunningVolume * ActionEffectsVolume);
            }
            else if (PlayerMovement.sliding && !PlayerMovement.OnSlope())
            {
                if (!PlayerMovementAudioSource.isPlaying)
                    PlayerMovementAudioSource.PlayOneShot(SlidingOnGroundSoundEffect, SlidingVolume * ActionEffectsVolume);
            }
            else
            {
                PlayerMovementAudioSource.Stop();
                PlayerMovementAudioSource.pitch = 1f;
            }
        }
        else
        {
            PlayerMovementAudioSource.Pause();
        }
    }

    public void WeaponsSoundEffects()
    {
        if (!mainMenu.InPauseMenu)
        {
            if (gunShot.IsShooting)
            {
                WeaponsAudioSource.pitch = GunShotPitch;
                WeaponsAudioSource.PlayOneShot(GunShotSoundEffect, GunShotVolume * WeaponsVolume);
            }
            else if (Swing.Swinging)
            {
                if (!WeaponsAudioSource.isPlaying)
                    WeaponsAudioSource.PlayOneShot(SwingSoundEffect, SwingVolume * WeaponsVolume);
            }
            else if (gunShot.isReloading)
            {
                WeaponsAudioSource.pitch = ReloadPitch;

                if (!WeaponsAudioSource.isPlaying)
                    WeaponsAudioSource.PlayOneShot(ReloadSoundEffect, ReloadVolume * WeaponsVolume);
            }
            else if (gunShot.ErrorFullMag)
            {
                if (!WeaponsAudioSource.isPlaying)
                    WeaponsAudioSource.PlayOneShot(ErrorSoundEffect, ErrorVolume * SoundEffectVolume);
            }
            else if (!SwapGun.InGunSlot)
            {
                if (Input.GetKeyDown(KeybindManager.GetKeyCode("GunSlot")))
                {
                    WeaponsAudioSource.PlayOneShot(ReloadSoundEffect, ReloadVolume * WeaponsVolume);
                }
            }
            else if (!SwapGun.InSowrdSlot)
            {
                if (Input.GetKeyDown(KeybindManager.GetKeyCode("SwordSlot")))
                {
                    WeaponsAudioSource.PlayOneShot(SwingSoundEffect, SwingVolume * WeaponsVolume);
                }
            }
        }
        else
        {
            WeaponsAudioSource.Pause();
        }
    }

    public void SpeedingSoundEffects()
    {
        if (!mainMenu.InPauseMenu)
        {
            if (PlayerMovement.sliding && PlayerMovement.OnSlope())
            {
                SlidingSpeedingPitch += Time.deltaTime * 0.1f;

                SpeedingAudioSource.pitch = SlidingSpeedingPitch;

                if (!SpeedingAudioSource.isPlaying)
                    SpeedingAudioSource.PlayOneShot(SpeedingSoundEffect, SpeedingVolume * SoundEffectVolume);
            }
            else if (PlayerMovement.Dashing)
            {
                SpeedingAudioSource.loop = false;
                SpeedingAudioSource.pitch = DashingPitch;

                if (!SpeedingAudioSource.isPlaying)
                    SpeedingAudioSource.PlayOneShot(DashingSoundEffect, SpeedingVolume * SoundEffectVolume);
            }
            else
            {
                SpeedingAudioSource.Stop();
                SlidingSpeedingPitch = NormalSlidingSpeedingPitch;
                SpeedingAudioSource.loop = true;
            }
        }
        else
        {
            SpeedingAudioSource.Pause();
        }
    }

    public void ParticleSoundEffect()
    {
        if (!mainMenu.InPauseMenu)
        {
            if (Input.GetKeyDown(KeybindManager.GetKeyCode("SlowMotion")))
            {
                TimeManagePitchChange();

                if (!ParticlesAudioSource.isPlaying)
                    ParticlesAudioSource.PlayOneShot(EnterSloMoSoundEffect, SlowMotionVolume * SoundEffectVolume);
            }
            else if (Input.GetKeyUp(KeybindManager.GetKeyCode("SlowMotion")) || Time.timeScale == 1f && TimeManage.WasInSlowmotion)
            {
                Invoke(nameof(Reset), ExitSloMoSoundEffect.length);
            }
        }
        else
        {
            ParticlesAudioSource.Pause();
        }
    }

    public void JumpsSoundEffects()
    {
        if (PlayerMovement.OnJumpPad)
        {
            JumpsAudioSource.PlayOneShot(JumpPadSoundEffect, JumpPadVolume * SoundEffectVolume);
        }
    }

    public void TimeManagePitchChange()
    {
        SprintingPitch *= TimeManage.SlowDownFactor;
        WalkingPitch *= TimeManage.SlowDownFactor;
        WallRunningPitch *= TimeManage.SlowDownFactor;
        WallRunningSpeedingPitch *= TimeManage.SlowDownFactor;
        CrouchingPitch *= TimeManage.SlowDownFactor;
        SlidingSpeedingPitch *= TimeManage.SlowDownFactor;
        ReloadPitch *= TimeManage.SlowDownFactor;
        DashingPitch *= TimeManage.SlowDownFactor;
        GunShotPitch *= TimeManage.SlowDownFactor;
        JumpPadPitch *= TimeManage.SlowDownFactor;
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

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("BackGroundVolume", BackGroundVolume);
        PlayerPrefs.SetFloat("ActionEffectsVolume", ActionEffectsVolume);
        PlayerPrefs.SetFloat("SoundEffectVolume", SoundEffectVolume);
        PlayerPrefs.SetFloat("WeaponsVolume", WeaponsVolume);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        BackGroundVolume = PlayerPrefs.GetFloat("BackGroundVolume", 1f);
        ActionEffectsVolume = PlayerPrefs.GetFloat("ActionEffectsVolume", 1f);
        SoundEffectVolume = PlayerPrefs.GetFloat("SoundEffectVolume", 1f);
        WeaponsVolume = PlayerPrefs.GetFloat("WeaponsVolume", 1f);

        BackGroundVolumeSlider.value = BackGroundVolume;
        ActionEffectsVolumeSlider.value = ActionEffectsVolume;
        SoundEffectVolumeSlider.value = SoundEffectVolume;
        WeaponsVolumeSlider.value = WeaponsVolume;
    }

    private void OnApplicationQuit()
    {
        SaveSettings();
    }
}
