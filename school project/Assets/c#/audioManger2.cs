using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManger2 : MonoBehaviour
{
    public AudioSource AS;
    public AudioClip bossMusic;
    // Start is called before the first frame update
    void Start()
    {
        AS.clip = bossMusic;
        AS.Play();
    }

    // Update is called once per frame
    void Update()
    {


    }
}
