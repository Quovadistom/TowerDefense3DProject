using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollRectWithSnap : MonoBehaviour, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.LogError(1);
    }
}
