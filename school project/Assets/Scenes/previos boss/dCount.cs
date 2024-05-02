using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class dCount : MonoBehaviour
{
    public Text dText;
    private float numtext;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        numtext = FindAnyObjectByType<respawn>().dCount;

        dText.text = numtext.ToString();

    }
}
