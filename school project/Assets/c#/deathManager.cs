using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class deathManager : MonoBehaviour
{
    public Transform resPos;
    public bool ifDead = false;
    public Rigidbody rb;

    [Header("deathCount")]
    public int deathCount = 0;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        

    }


    // Update is called once per frame
    void Update()
    {
        text.text = deathCount.ToString();
        
        if (ifDead)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = resPos.position;
            transform.rotation = resPos.rotation;
            deathCount++;
            ifDead = false;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "death")
        {
            ifDead = true;
        }
        if(other.gameObject.tag == "checkpoint")
        {
            resPos = other.transform;
        }
    }
}
