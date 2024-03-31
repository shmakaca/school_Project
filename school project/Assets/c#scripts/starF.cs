using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class starF : MonoBehaviour
{
    public Object stars;
    private int StarSpeed;

    public bool ifSF = true;

    public Object player;

    public int x;
    public int z;
    // Start is called before the first frame update
    void Start()
    {
        if (ifSF)
        {
            for(int i = 0; i < 3; i++)
            {
                //Vector3 pp = player.GetComponent<Vector3>();
                Vector3 randomPos = new Vector3(Random.Range( - x , + x) ,  100 ,Random.Range( -z,  +z));


                Instantiate(stars, randomPos, Quaternion.identity);
            
            
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
        

    }

    
}
