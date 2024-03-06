using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using Zenject;

public class TouchInputService
{
    public bool IsSingleTouchActive() => Input.touchCount == 1;

    public bool TryGetRaycast(LayerMask layerMask, out RaycastHit hit)
    {
        hit = default;

        if (!IsSingleTouchActive())
        {
            return false;
        }

        Touch touch = Input.touches[0];
        Vector3 pos = touch.position;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        return Physics.Raycast(ray, out hit, 1000, layerMask);
    }

    public bool TryGetTouchPhase(out TouchPhase touchPhase)
    {
        touchPhase = default;

        if (IsSingleTouchActive())
        {
            Touch touch = Input.touches[0];
            touchPhase = touch.phase;
        }

        return IsSingleTouchActive();
    }
}
