using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TimeMange : MonoBehaviour
{
    [Header("Time Mange")]
    public float SlowDownFactor;// how much time willslow down
    public float BackToNormalTime;
    public float Focus;
    private float SlowDownLenght;
    public bool InSlowMotion;
    private bool ReadyToSlowMotion;

    [Header("Inputs")]
    public MouseButton SLowMotionKey = MouseButton.RightMouse;

    private void FixedUpdate()
    {
        if (Input.GetMouseButton((int)SLowMotionKey))
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

        if (SlowDownLenght >= Focus)
        {
            ReadyToSlowMotion = false;
            Input.GetMouseButtonUp((int)SLowMotionKey);
            SlowDownLenght = Focus;
        }

        if (ReadyToSlowMotion && SlowDownLenght < Focus)
        {
            InSlowMotion = true;
            SlowMotion();
        }
        else
            InSlowMotion = false;



    }
    void Update()
    {
        Time.timeScale += (1 / BackToNormalTime) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    private void SlowMotion()
    {

        Time.timeScale = SlowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

    }
}
