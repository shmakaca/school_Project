using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TimeMange : MonoBehaviour
{
    [Header("Time Mange")]
    public float SlowDownFactor;// how much time willslow down
    private float BackToNormalTime;
    public float Focus;
    private float SlowDownLenght;
    public bool InSlowMotion;
    private bool ReadyToSlowMotion;
    public bool WasInSlowmotion = false;

    [Header("Reafrences")]
    private AudioApply AudioAplly;
    public GameObject AM;
    public GameObject KeyBindMenu;
    private KeybindManager KeybindManager;

    private void Start()
    {
        AudioAplly = AM.GetComponent<AudioApply>();

        BackToNormalTime = AudioAplly.ExitSloMoSoundEffect.length;

        KeybindManager = KeyBindMenu.GetComponent<KeybindManager>();
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeybindManager.GetKeyCode("SlowMotion")))
        {
            ReadyToSlowMotion = true;
            SlowDownLenght += Time.deltaTime * (1 / SlowDownFactor);
        }

        else
        {
            ReadyToSlowMotion = false;
            SlowDownLenght -= Time.deltaTime * (1 / SlowDownFactor);
        }

        if (SlowDownLenght < 0)
            SlowDownLenght = 0;

        if (SlowDownLenght >= Focus )
        {
            ReadyToSlowMotion = false;
            SlowDownLenght = Focus;
        }
        if (Input.GetKeyUp(KeybindManager.GetKeyCode("SlowMotion")))
        {
            ReadyToSlowMotion = false;
        }

        if (ReadyToSlowMotion && SlowDownLenght <= Focus)
        {
            SlowMotion();
        }
        else
            InSlowMotion = false;

        if (Time.timeScale == 1f && WasInSlowmotion)
        {
            WasInSlowmotion = false;

            AudioAplly.ParticlesAudioSource.PlayOneShot(AudioAplly.ExitSloMoSoundEffect, 0.1f);

            Invoke(nameof(AudioAplly.Reset), AudioAplly.ExitSloMoSoundEffect.length);
        }

        else if (Time.timeScale < 1f)
        {
            WasInSlowmotion = true; 
        }

    }
    void Update()
    {
        Time.timeScale += (1 / BackToNormalTime) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    private void SlowMotion()
    {
        InSlowMotion = true;
        Time.timeScale = SlowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

    }

}

