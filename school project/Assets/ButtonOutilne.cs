using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ButtonOutlineHighlighter : MonoBehaviour
{
    // List of buttons to check
    public List<Button> buttons;

    // Material for highlighted state
    public Material highlightMaterial;

    // Original material to revert when the button is not highlighted
    private Dictionary<Button, Material> originalMaterials = new Dictionary<Button, Material>();

    void Start()
    {
        // Ensure each button has an EventTrigger and store original materials
        foreach (Button button in buttons)
        {
            // Store the original material of the button
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                originalMaterials[button] = buttonImage.material;
            }

            // Add EventTrigger component if not present
            EventTrigger eventTrigger = button.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                eventTrigger = button.gameObject.AddComponent<EventTrigger>();
            }

            // Add pointer enter event
            AddEventTriggerListener(eventTrigger, EventTriggerType.PointerEnter, () => OnButtonHighlight(button, true));
            // Add pointer exit event
            AddEventTriggerListener(eventTrigger, EventTriggerType.PointerExit, () => OnButtonHighlight(button, false));
        }
    }

    void AddEventTriggerListener(EventTrigger eventTrigger, EventTriggerType eventType, System.Action action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((eventData) => action());
        eventTrigger.triggers.Add(entry);
    }

    void OnButtonHighlight(Button button, bool highlight)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.material = highlight ? highlightMaterial : originalMaterials[button];
        }
    }
}
