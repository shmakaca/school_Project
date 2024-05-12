using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardController : MonoBehaviour
{
    [Header ("KeyCodes")]
    public KeyCode Sowrdkc;
    public KeyCode gunck;
    public KeyCode jumpck;
    public KeyCode wallRunUP;
    public KeyCode wallRunDown;
    public KeyCode slideck;
    public KeyCode dashck;
    public KeyCode runck;


    [Header ("Inputs")]
    public TMP_InputField sowrdIn;
    public InputField gunIN;
    public InputField jumpIN;
    public InputField wallRunUPIN;
    public InputField wallRunDownIN;
    public InputField slideIN;
    public InputField dashIN;
    public InputField runIN;
    // Start is called before the first frame update
    void Start()
    {
        DefaultCK();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DefaultCK() 
    {
        Sowrdkc = KeyCode.Alpha1;
        gunck = KeyCode.Alpha2;
        jumpck = KeyCode.Space;
        wallRunDown = KeyCode.Q;
        wallRunUP = KeyCode.E;
        slideck = KeyCode.LeftControl;
        dashck = KeyCode.E;
        runck = KeyCode.LeftShift;

    }
    public KeyCode GetSowrd()
    {
        return Sowrdkc;
    }
    public void SetSowrd()
    {
        char x = sowrdIn.text[0];

        Sowrdkc = sowrdIn.text[0].ConvertTo<KeyCode>();
    }
}
