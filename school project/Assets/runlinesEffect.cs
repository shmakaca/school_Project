using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runlinesEffect : MonoBehaviour
{
    PlayerMovement PlayerMovement;
    Dash Dash;
    public ParticleSystem SpeedLines;
    public ParticleSystem SpeedLinesWhileDashing;
    public ParticleSystem SpeedLinesInMomentom;
    private void Start()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
        Dash = GetComponent<Dash>();
    }
    private void Update()
    {
        EffectToApply();
    }
    private void EffectToApply()
    {
        if (PlayerMovement.Dashing)
        {
            SpeedLinesWhileDashing.Play();

        }
        if(PlayerMovement.WallRunning)
        {
            SpeedLines.loop = true;
            SpeedLines.Play();
        }
        else
        {
            SpeedLines.loop = false;
        }
        if(PlayerMovement.MoveSpeed > 15f && PlayerMovement.MoveSpeed <20f)
        {
            SpeedLinesInMomentom.Play();
            SpeedLinesInMomentom.loop = true;
        }
        else if(PlayerMovement.MoveSpeed >= 20f && PlayerMovement.MoveSpeed < 30f)
        {
            SpeedLinesInMomentom.loop = true;
            SpeedLinesWhileDashing.loop = true;
            SpeedLinesInMomentom.Play();
            SpeedLinesWhileDashing.Play();
            
        }
        else if ( PlayerMovement.MoveSpeed >= 30f)
        {
            SpeedLinesInMomentom.loop = true;
            SpeedLines.loop = true;
            SpeedLinesWhileDashing.loop = true;
            SpeedLinesInMomentom.Play();
            SpeedLinesWhileDashing.Play();
            SpeedLines.Play();
            
        }
        else
        {
            SpeedLines.loop = false;
            SpeedLinesWhileDashing.loop = false;
            SpeedLinesInMomentom.loop = false;
        }
    }


}
