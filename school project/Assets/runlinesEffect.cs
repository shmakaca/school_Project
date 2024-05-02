using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runlinesEffect : MonoBehaviour
{
    PlayerMovement PlayerMovement;
    public ParticleSystem SpeedLines;
    public ParticleSystem SpeedLinesInMomentom;
    public ParticleSystem SpeedLinesInFastMomentom;

    private bool InLoop;
    private void Start()
    {
        PlayerMovement = GetComponent<PlayerMovement>();

        SpeedLines.Stop();
        SpeedLinesInMomentom.Stop();
        SpeedLinesInFastMomentom.Stop();


    }
    private void Update()
    {
        EffectToApplyWhileFastSpeeding();;
        EffectToApplyWhileWallRunning();
        EffectToApplyWhileSliding();
    }

    private void EffectToApplyWhileWallRunning()
    {
        var SpeedLinesmain = SpeedLines.main;

        if (PlayerMovement.WallRunning)
        {
            SpeedLinesmain.loop = true;
            SpeedLines.Play();  
        }
        else
        {
            SpeedLinesmain.loop = false;
        }

    }
    private void EffectToApplyWhileFastSpeeding()
    {
        var SpeedLinesInFastMomentomain = SpeedLinesInFastMomentom.main;

        if (PlayerMovement.MoveSpeed >= 17f && PlayerMovement.sliding && PlayerMovement.OnSlope())
        {
            SpeedLinesInFastMomentomain.loop = true;    
            SpeedLinesInFastMomentom.Play();
        }
        else
        {
            SpeedLinesInFastMomentomain.loop = false;
        }
    }
    private void EffectToApplyWhileSliding()
    {
        var SpeedLinesInMomentomain = SpeedLinesInMomentom.main;

        if (PlayerMovement.sliding && PlayerMovement.OnSlope()  && PlayerMovement.MoveSpeed >= 12f && PlayerMovement.MoveSpeed < 17f  )
        {
            SpeedLinesInMomentomain.loop = true;
            SpeedLinesInMomentom.Play();    
        }
        else
        {
            SpeedLinesInMomentomain.loop= false;
        }
    }
    



}
