using UnityEngine.UI;

public class ShapedButton : Button
{
    protected override void Awake()
    {
        if (image != null)
        {
            image.alphaHitTestMinimumThreshold = 0.1f;
        }
    }
}
