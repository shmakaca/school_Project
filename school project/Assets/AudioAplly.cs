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
    private GunShot gunShot;
    private PlayerDamaging Swing;
    private SwapGun SwapGun;
    private static MainMenu mainMenu;
    private static TimeMange TimeManage;
    private KeybindManager KeybindManager;

    [Header("Sliders References")]
    public Slider BackGroundVolumeSlider;
    public Slider PlayerActionsVolumeSlider;
    public Slider SoundEffectVolumeSlider;
    public Slider MasterVolumeSlider;

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
    private float PlayerActionsVolume = 0f;
    private float SoundEffectVolume = 0f;
    private float MasterVolume = 0f;

    void Start()
    {
        TimeManage = Player.GetComponent<TimeMange>();
        PlayerMovement = Player.GetComponent<PlayerMovement>();
        gunShot = Player.GetComponent<GunShot>();
        Swing = Sword.GetComponent<PlayerDamaging>();
        SwapGun = Player.GetComponent<SwapGun>();
        mainMenu = MainMenu.GetComponent<MainMenu>();
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
        BackGroundVolume = BackGroundVolumeSlider.value / 20f;
        PlayerActionsVolume = PlayerActionsVolumeSlider.value / 20f;
        SoundEffectVolume = SoundEffectVolumeSlider.value / 20f;
        MasterVolume = MasterVolumeSlider.value / 100f;
    }

    public void BackGroundMusic()
    {
        if (!BackGroundAudioSource.isPlaying)
        {
            BackGroundAudioSource.Play();
            BackGroundAudioSource.volume = bossMusicVolume * BackGroundVolume * MasterVolume;
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
                    PlayerMovementAudioSource.PlayOneShot(WalkingSoundEffect, WalkingVolume * PlayerActionsVolume * MasterVolume);
            }
            else if (PlayerMovement.State == PlayerMovement.MovementState.Sprinting && (PlayerMovement.Horizontal != 0 || PlayerMovement.Vertical != 0))
            {
                PlayerMovementAudioSource.pitch = SprintingPitch;

                if (!PlayerMovementAudioSource.isPlaying)
                    PlayerMovementAudioSource.PlayOneShot(WalkingSoundEffect, SprintingVolume * PlayerActionsVolume * MasterVolume);
            }
            else if (PlayerMovement.State == PlayerMovement.MovementState.Crouching && (PlayerMovement.Horizontal != 0 || PlayerMovement.Vertical != 0))
            {
                PlayerMovementAudioSource.pitch = CrouchingPitch;

                if (!PlayerMovementAudioSource.isPlaying)
                    PlayerMovementAudioSource.PlayOneShot(WalkingSoundEffect, CrouchingVolume * PlayerActionsVolume * MasterVolume);
            }
            else if (PlayerMovement.WallRunning)
            {
                PlayerMovementAudioSource.pitch = WallRunningPitch;

                if (!PlayerMovementAudioSource.isPlaying)
                    PlayerMovementAudioSource.PlayOneShot(WalkingSoundEffect, WallRunningVolume * PlayerActionsVolume * MasterVolume);
            }
            else if (PlayerMovement.Sliding && !PlayerMovement.OnSlope())
            {
                if (!PlayerMovementAudioSource.isPlaying)
                    PlayerMovementAudioSource.PlayOneShot(SlidingOnGroundSoundEffect, SlidingVolume * PlayerActionsVolume * MasterVolume);
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
            if (PlayerMovement.Sliding && PlayerMovement.OnSlope())
            {
                SlidingSpeedingPitch += Time.deltaTime * 0.1f;

                SpeedingAudioSource.pitch = SlidingSpeedingPitch;

                if (!SpeedingAudioSource.isPlaying)
                    SpeedingAudioSource.PlayOneShot(SpeedingSoundEffect, SpeedingVolume * SoundEffectVolume * MasterVolume);
            }
            else if (PlayerMovement.Dashing)
            {
                SpeedingAudioSource.loop = false;
                SpeedingAudioSource.pitch = DashingPitch;

                if (!SpeedingAudioSource.isPlaying)
                    SpeedingAudioSource.PlayOneShot(DashingSoundEffect, SpeedingVolume * SoundEffectVolume * MasterVolume);
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
                    ParticlesAudioSource.PlayOneShot(EnterSloMoSoundEffect, SlowMotionVolume * SoundEffectVolume * MasterVolume);
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
            JumpsAudioSource.PlayOneShot(JumpPadSoundEffect, JumpPadVolume * SoundEffectVolume * MasterVolume);
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
        PlayerPrefs.SetFloat("ActionEffectsVolume", PlayerActionsVolume);
        PlayerPrefs.SetFloat("SoundEffectVolume", SoundEffectVolume);
        PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        BackGroundVolume = PlayerPrefs.GetFloat("BackGroundVolume", 1f);
        PlayerActionsVolume = PlayerPrefs.GetFloat("ActionEffectsVolume", 1f);
        SoundEffectVolume = PlayerPrefs.GetFloat("SoundEffectVolume", 1f);
        MasterVolume = PlayerPrefs.GetFloat("WeaponsVolume", 1f);

        BackGroundVolumeSlider.value = BackGroundVolume;
        PlayerActionsVolumeSlider.value = PlayerActionsVolume;
        SoundEffectVolumeSlider.value = SoundEffectVolume;
        MasterVolumeSlider.value = MasterVolume;
    }

    private void OnApplicationQuit()
    {
        SaveSettings();
    }
}
