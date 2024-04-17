using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class desIno : MonoBehaviour
{
    private float InoCD = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InoCD > 0)
        {
            InoCD = InoCD - Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
