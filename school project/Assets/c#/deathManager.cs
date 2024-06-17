using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class deathManager : MonoBehaviour
{
    public Transform resPos;
    public bool ifDead = false;
    public Rigidbody rb;
    public GameObject player;

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
        if (Input.GetKeyDown(KeyCode.F))
        {

            ifDead = true;
        }
        if (ifDead)
        {
            player.SetActive(false);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            this.transform.position = this.resPos.position;
            this.transform.rotation = this.resPos.rotation;
            deathCount++;
            
            
            player.SetActive(true);
            ifDead = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "death")
        {
            ifDead = true;
        }
        if (other.gameObject.tag == "checkpoint")
        {
            resPos = other.transform;
        }
    }
}
