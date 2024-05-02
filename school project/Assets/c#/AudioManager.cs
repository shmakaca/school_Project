using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using TMPro;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource music;

    public AudioClip def;
    public AudioClip barn;


    public bool isDef = true;


    // Start is called before the first frame update
    void Start()
    {
        music.clip = def;
        music.Play();



    }

    // Update is called once per frame
    void Update()
    {



    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "audioBarn")
        {
            isDef = true;
            music.Pause();
            music.clip = barn;
            music.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "audioBarn")
        {
            music.Pause();
            music.clip = def;
            music.Play();
            isDef = true;
        }
    }
}
