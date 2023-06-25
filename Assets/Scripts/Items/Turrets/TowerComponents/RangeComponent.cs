using System;
using UnityEngine;

[Serializable]
public class RangeComponent : ComponentBase
{
    public ComponentDataTypeFloatWithBuff RangeValue;
    public ComponentDataTypeVisual<Transform> VisualComponent;
}
