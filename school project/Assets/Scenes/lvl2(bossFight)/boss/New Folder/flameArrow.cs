using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameArrow : MonoBehaviour
{
    public GameObject player;
    private float cd = 4;
    Vector3 playerPos;
    public Rigidbody rb;
    public int speed = 6;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Fall>().player;

        playerPos = player.transform.position;
        transform.LookAt(playerPos);

        Vector3 dir = playerPos - transform.position;
        rb.AddForce(dir * speed / 100 * Random.Range(0.9f, 1.1f), ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {



        if (cd < 0)
        {
            Destroy(gameObject);

        }
        else
        {
            cd = cd - Time.deltaTime;
        }

    }

}
