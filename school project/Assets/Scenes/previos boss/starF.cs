using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class starF : MonoBehaviour
{
    public Object stars;


    public bool ifSF;



    public int x;
    public int z;
    public int numFall = 5;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

        ifSF = FindObjectOfType<bossAttack>().isStarFall;
        while (ifSF == true)
        {

            for (int i = 0; i < numFall; i++)
            {

                Vector3 randomPos = new Vector3(Random.Range(-x, +x), Random.Range(90, 130), Random.Range(-z, +z));


                Instantiate(stars, randomPos, Quaternion.identity);


            }
            ifSF = false;
        }

    }


}
