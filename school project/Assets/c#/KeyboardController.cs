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
    public TMP_InputField gunIN;
    public TMP_InputField jumpIN;
    public TMP_InputField wallRunUPIN;
    public TMP_InputField wallRunDownIN;
    public TMP_InputField slideIN;
    public TMP_InputField dashIN;
    public TMP_InputField runIN;

    [Header("what is currently changing")]
    private bool IsSowrd = false;
    private bool IsGun = false;
    private bool IsJump = false;
    private bool IsWallup = false;
    private bool IsWalldown = false;
    private bool IsSlide = false;
    private bool IsDash = false;
    private bool IsRun = false;

    


    // Start is called before the first frame update
    void Start()
    {
        DefaultCK();
    }

    // Update is called once per frame
    void Update()
    {
        SetKeys();

    }
    public void SetKeys()
    {
        if (IsSowrd)
        {
            Sowrdkc = TheGreatFilter(sowrdIn.text);
        }
        if (IsGun)
        {
            gunck = TheGreatFilter(gunIN.text);
        }
        if (IsJump)
        {
            jumpck = TheGreatFilter(jumpIN.text);
        }
        if (IsWallup)
        {
            wallRunUP = TheGreatFilter(wallRunUPIN.text);
        }
        if (IsWalldown)
        {
            wallRunDown = TheGreatFilter(wallRunDownIN.text);
        }
        if (IsSlide)
        {
            slideck = TheGreatFilter(slideIN.text);
        }
        if (IsDash)
        {
            dashck = TheGreatFilter(dashIN.text);
        }
        if (IsRun)
        {
            runck = TheGreatFilter(runIN.text);
        }
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


        sowrdIn.text = Sowrdkc.ToString();
        gunIN.text = gunck.ToString();
        jumpIN.text = jumpck.ToString();
        wallRunUPIN.text = wallRunUP.ToString();
        wallRunDownIN.text = wallRunDown.ToString();
        slideIN.text = slideck.ToString();
        dashIN.text = dashck.ToString();
        runIN.text = runck.ToString();


    }
    public void checkSowrdSet()
    {
        IsSowrd = true;
        IsGun = false ;
        IsJump = false ;
        IsWallup = false ;
        IsWalldown = false ;
        IsSlide = false ;
        IsDash = false ;
        IsRun = false ;

    }
    public void checkGunSet()
    {
        IsSowrd = false;
        IsGun = true;
        IsJump = false;
        IsWallup = false;
        IsWalldown = false;
        IsSlide = false;
        IsDash = false;
        IsRun = false;

    }
    public void checkjumpSet()
    {
        IsSowrd = false;
        IsGun = false;
        IsJump = true;
        IsWallup = false;
        IsWalldown = false;
        IsSlide = false;
        IsDash = false;
        IsRun = false;

    }
    public void checkWallUpSet()
    {
        IsSowrd = false;
        IsGun = false;
        IsJump = false;
        IsWallup = true;
        IsWalldown = false;
        IsSlide = false;
        IsDash = false;
        IsRun = false;

    }
    public void checkWallDownSet()
    {
        IsSowrd = false;
        IsGun = false;
        IsJump = false;
        IsWallup = false;
        IsWalldown = true;
        IsSlide = false;
        IsDash = false;
        IsRun = false;

    }
    public void checkSlideSet()
    {
        IsSowrd = false;
        IsGun = false;
        IsJump = false;
        IsWallup = false;
        IsWalldown = false;
        IsSlide = true;
        IsDash = false;
        IsRun = false;

    }
    public void checkDashSet()
    {
        IsSowrd = false;
        IsGun = false;
        IsJump = false;
        IsWallup = false;
        IsWalldown = false;
        IsSlide = false;
        IsDash = true;
        IsRun = false;

    }
    public void checkRunSet()
    {
        IsSowrd = false;
        IsGun = false;
        IsJump = false;
        IsWallup = false;
        IsWalldown = false;
        IsSlide = false;
        IsDash = false;
        IsRun = true;

    }
    public KeyCode TheGreatFilter(string input)
    {
        if(input != null)
        {
            if (input == "Lcontrol" || input == "lcontrol" || input == "LeftControl" || input == "leftcontrol")
            {
                return KeyCode.LeftControl;
            }
            if (input == "Lshift" || input == "lshift" || input == "LeftShift" || input == "leftshift")
            {
                return KeyCode.LeftShift;
            }
            if (input == "Capslock" || input == "capslock" || input == "CapsLock" || input == "capsLock")
            {
                return KeyCode.CapsLock;
            }
            if (input == "Tab" || input == "tab" || input == "TAB" || input == "LeftTab")
            {
                return KeyCode.Tab;
            }
            if (input == "Space" || input == "space" || input == "Jump" || input == "jump")
            {
                return KeyCode.Space;
            }
            if (input[0] == 'a' || input[0] == 'A')
            {
                return KeyCode.A;
            }
            if (input[0] == 'b' || input[0] == 'B')
            {
                return KeyCode.B;
            }
            if (input[0] == 'c' || input[0] == 'C')
            {
                return KeyCode.C;
            }
            if (input[0] == 'D' || input[0] == 'd')
            {
                return KeyCode.D;
            }
            if (input[0] == 'e' || input[0] == 'E')
            {
                return KeyCode.E;
            }
            if (input[0] == 'f' || input[0] == 'F')
            {
                return KeyCode.F;
            }
            if (input[0] == 'g' || input[0] == 'G')
            {
                return KeyCode.G;
            }
            if (input[0] == 'h' || input[0] == 'H')
            {
                return KeyCode.H;
            }
            if (input[0] == 'I' || input[0] == 'i')
            {
                return KeyCode.I;
            }
            if (input[0] == 'j' || input[0] == 'J')
            {
                return KeyCode.J;
            }
            if (input[0] == 'k' || input[0] == 'K')
            {
                return KeyCode.K;
            }
            if (input[0] == 'l' || input[0] == 'L')
            {
                return KeyCode.L;
            }
            if (input[0] == 'm' || input[0] == 'M')
            {
                return KeyCode.M;
            }
            if (input[0] == 'n' || input[0] == 'N')
            {
                return KeyCode.N;
            }
            if (input[0] == 'o' || input[0] == 'O')
            {
                return KeyCode.O;
            }
            if (input[0] == 'p' || input[0] == 'P')
            {
                return KeyCode.P;
            }
            if (input[0] == 'q' || input[0] == 'Q')
            {
                return KeyCode.Q;
            }
            if (input[0] == 'r' || input[0] == 'R')
            {
                return KeyCode.R;
            }
            if (input[0] == 's' || input[0] == 'S')
            {
                return KeyCode.A;
            }
            if (input[0] == 't' || input[0] == 'T')
            {
                return KeyCode.T;
            }
            if (input[0] == 'u' || input[0] == 'U')
            {
                return KeyCode.U;
            }
            if (input[0] == 'v' || input[0] == 'V')
            {
                return KeyCode.V;
            }
            if (input[0] == 'w' || input[0] == 'W')
            {
                return KeyCode.W;
            }
            if (input[0] == 'X' || input[0] == 'x')
            {
                return KeyCode.X;
            }
            if (input[0] == 'y' || input[0] == 'Y')
            {
                return KeyCode.Y;
            }
            if (input[0] == 'z' || input[0] == 'Z')
            {
                return KeyCode.Z;
            }
            if (input[0] == '1')
            {
                return KeyCode.Alpha1;
            }
            if (input[0] == '2' )
            {
                return KeyCode.Alpha2;
            }
            if (input[0] == '3' )
            {
                return KeyCode.Alpha3;
            }
            if (input[0] == '4' )
            {
                return KeyCode.Alpha4;
            }
            if (input[0] == '5' )
            {
                return KeyCode.Alpha5;
            }
            if (input[0] == '6' )
            {
                return KeyCode.Alpha6;
            }
            if (input[0] == '7' )
            {
                return KeyCode.Alpha7;
            }
            if (input[0] == '8' )
            {
                return KeyCode.Alpha8;
            }
            if (input[0] == '9' )
            {
                return KeyCode.Alpha9;
            }
            if (input[0] == '0' )
            {
                return KeyCode.Alpha0;
            }
            

            return KeyCode.Mouse6;
        }

        return KeyCode.Mouse6;
    } //takes string and convert into key code (its built with alot of if statements ,i didnt find a better way)
}
