using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorSettings", menuName = "ScriptableObjects/ColorSettings")]
public class ColorSettings : ScriptableObject
{
    public Color RangeFreeToPlaceColor;
    public Color RangeBlockedToPlaceColor;

    [Header("Outline Colors")]
    public Color DefaultOutline;
    public Color DoubleClickOutline;
    public Color FocusOutline;
    public Color ConnectedOutline;
}
