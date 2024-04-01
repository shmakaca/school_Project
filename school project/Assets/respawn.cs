using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class respawn : MonoBehaviour
{
    public Transform player ,res;
    public GameObject Player;
    
    public void OnTriggerEnter(Collider co)
    {
        if (co.gameObject.tag == "death")
        {
            Player.SetActive(false);
            player.position = res.position;
            Player.SetActive(true);

        }
    }
}
