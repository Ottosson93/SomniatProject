using UnityEngine;
using UnityEngine.UI;

public class MaskUpdater : MonoBehaviour
{
    public RectTransform maskImage;
    public RectTransform boundaryImage;

    private void Update()
    {
        // Update the position and size of the mask to match the circular boundary
        maskImage.position = boundaryImage.position;
        maskImage.sizeDelta = boundaryImage.sizeDelta;
    }
}
