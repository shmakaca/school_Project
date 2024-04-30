using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [Header("Rearences")]
    private PlayerMovement PlayerMovement;
    public AudioSource Src;
    public AudioClip Walking;
    public AudioClip Wind;
    public AudioClip GunShot;
    public AudioClip Dashing;
    public AudioClip Swinging;

    void Start()
    {
        PlayerMovement = GetComponent<PlayerMovement>(); 
    }

    // Update is called once per frame
    void Update()
    {
        ApplyEffects();
    }

    public void ApplyEffects()
    {
        if (PlayerMovement.State == PlayerMovement.MovementState.Walking)
        {
            Src.clip = Walking;
            Src.Play();
        }
        else if (PlayerMovement.State == PlayerMovement.MovementState.Dashing)
        {
            Src.clip = Dashing;
            Src.Play();
        }
        else if (PlayerMovement.State == PlayerMovement.MovementState.Sliding)
        {
            Src.clip = Wind;
            Src.Play();
        }
        else if (PlayerMovement.State == PlayerMovement.MovementState.WallRunning)
        {
            Src.clip = Wind;
            Src.Play();
        }

    }
}
