using System.Collections.Generic;
using UnityEngine.EventSystems;

public static class EventSystemExtensions
{
    public static bool IsUILayerSelected(this PointerEventData pointerEventData)
    {
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(pointerEventData, results);
        return results.Count > 0;
    }
}
