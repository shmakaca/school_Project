using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    public GameObject Farrows;
    public GameObject player;
    bool isFall;

    [Header("stats")]
    public int shotsNum;

    public float CD;

    private float FcountDown;



    // Start is called before the first frame update
    void Start()
    {
        FcountDown = CD;
    }

    // Update is called once per frame
    void Update()
    {

        if (FcountDown < 0)
        {
            isFall = true;
            FcountDown = CD;
        }
        else
        {
            FcountDown = FcountDown - Time.deltaTime;
        }


        if (isFall)
        {

            for (int i = 0; i < shotsNum; i++)
            {
                bool can = true;
                if (can)
                {
                    summon();
                    can = false;
                }

            }
            isFall = false;
        }
    }
    void summon()
    {
        Instantiate(Farrows, transform.position * Random.Range(0.9f, 1.3f), Quaternion.identity);
    }

}
