
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class respawn : MonoBehaviour
{
    public Transform player, res;
    public GameObject Player;
    public float dCount = 0;

    public bool isDeathReady;
    public float dTime = 5;

    public void Update()
    {
        if (dTime > 0)
        {
            isDeathReady = false;
            dTime = dTime - Time.deltaTime;
        }
        else
        {
            isDeathReady = true;

        }
    }
    public void OnTriggerEnter(Collider co)
    {
        if (co.gameObject.tag == "death")
        {
            respon();
            if (isDeathReady)
            {
                dCount++;
                isDeathReady = false;
            }


        }
    }
    public void respon()
    {
        Player.SetActive(false);
        player.position = res.position;
        Player.SetActive(true);
    }
}