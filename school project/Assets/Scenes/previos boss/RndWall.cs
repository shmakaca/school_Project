using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RndWall : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    private float wallCountDown;
    public Vector3 wallPos;
    public float wallDesTime = 3;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {



        if (wallCountDown > 0)
        {

            wallCountDown = wallCountDown - Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                wallPos = new Vector3(1, 15, Random.Range(-110, 110));
                Instantiate(wall, wallPos, Quaternion.identity);
            }

            wallCountDown = 5;


        }
    }

}
