using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class bamboziled : MonoBehaviour
{
    public GameObject bamText;
    public bool isBam = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isBam)
        {
            bamText.SetActive(true);

            Invoke("stopBam", 3);
        }
    }
    private void stopBam()
    {
        bamText.SetActive(false);
        isBam = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isBam =true;
        }
    }
}
