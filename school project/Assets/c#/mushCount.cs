using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class mushCount : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int x = FindAnyObjectByType<mushTake>().mushNum;
        textMeshPro.text = x.ToString();
    }
}
