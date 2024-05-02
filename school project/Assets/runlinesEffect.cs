using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runlinesEffect : MonoBehaviour
{
    PlayerMovement PlayerMovement;
    public ParticleSystem SpeedLines;
    public ParticleSystem SpeedLinesWhileDashing;
    public ParticleSystem SpeedLinesInMomentom;

    private bool InLoop;
    private void Start()
    {
        PlayerMovement = GetComponent<PlayerMovement>();

        SpeedLines.Pause();
        SpeedLinesWhileDashing.Pause();
        SpeedLinesInMomentom.Pause();
    }
    private void Update()
    {
        EffectToApply();
    }
    private void EffectToApply()
    {
        var SpeedLinesmain = SpeedLines.main;
        var SpeedLinesWhileDashingmain = SpeedLinesWhileDashing.main;
        var SpeedLinesInMomentomain = SpeedLinesInMomentom.main;

        if (PlayerMovement.Dashing)
        {
            SpeedLinesWhileDashing.Play();  
        }
        else if (PlayerMovement.WallRunning)
        {
            SpeedLinesmain.loop = true;
            SpeedLines.Play();  
        }
        else if (PlayerMovement.MoveSpeed > 15f && PlayerMovement.MoveSpeed < 20f)
        {
            SpeedLinesInMomentomain.loop = true;  
            SpeedLinesInMomentom.Play();
        }
        else if (PlayerMovement.MoveSpeed >= 20f && PlayerMovement.MoveSpeed < 30f)
        {
            SpeedLinesInMomentomain.loop = true;
            SpeedLinesInMomentom.Play();
            SpeedLinesWhileDashingmain.loop = true;
            SpeedLinesWhileDashing.Play();

        }
        else
        {
            SpeedLinesmain.loop = false;
            SpeedLinesWhileDashingmain.loop = false;
            SpeedLinesInMomentomain.loop = false;
        }
    }


}
