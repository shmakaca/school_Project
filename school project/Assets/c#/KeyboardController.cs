using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class KeybindManager : MonoBehaviour
{
    public KeyCode JumpKey = KeyCode.Space;
    public KeyCode SprintKey = KeyCode.LeftShift;
    public KeyCode CrouchKey = KeyCode.LeftControl;

    public TMP_Text jumpButtonText;
    public TMP_Text sprintButtonText;
    public TMP_Text crouchButtonText;

    private TMP_Text currentButtonText;
    private bool isWaitingForKey;

    void Start()
    {
        UpdateButtonText();
    }

    void UpdateButtonText()
    {
        jumpButtonText.text = JumpKey.ToString();
        sprintButtonText.text = SprintKey.ToString();
        crouchButtonText.text = CrouchKey.ToString();
    }

    public void StartRebind(string action)
    {
        if (isWaitingForKey) return;
        StartCoroutine(WaitForKeyPress(action));
    }

    IEnumerator WaitForKeyPress(string action)
    {
        isWaitingForKey = true;

        switch (action)
        {
            case "Jump":
                currentButtonText = jumpButtonText;
                break;
            case "Sprint":
                currentButtonText = sprintButtonText;
                break;
            case "Crouch":
                currentButtonText = crouchButtonText;
                break;
        }

        currentButtonText.text = "Press any key to bind...";

        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                switch (action)
                {
                    case "Jump":
                        JumpKey = keyCode;
                        break;
                    case "Sprint":
                        SprintKey = keyCode;
                        break;
                    case "Crouch":
                        CrouchKey = keyCode;
                        break;
                }
                break;
            }
        }

        UpdateButtonText();
        isWaitingForKey = false;
    }
}
