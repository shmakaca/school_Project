using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movecamera : MonoBehaviour
{
    public Transform cameraPostion;
    void Update()
    {
        transform.position = cameraPostion.position;
    }
}
