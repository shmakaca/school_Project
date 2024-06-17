using UnityEngine;
using UnityEngine.UI;

public class ScrollToTop : MonoBehaviour
{
    public ScrollRect scrollRect;

    void OnEnable()
    {
        // Set the scroll position to the top
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }
}
