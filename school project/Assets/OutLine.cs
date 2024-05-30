using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonImageController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image targetImage; // Public variable to reference the child image

    void Start()
    {
        // Ensure the target image starts as invisible
        if (targetImage != null)
        {
            targetImage.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowImage();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideImage();
    }

    private void ShowImage()
    {
        if (targetImage != null)
        {
            targetImage.enabled = true;
        }
    }

    private void HideImage()
    {
        if (targetImage != null)
        {
            targetImage.enabled = false;
        }
    }
}
