using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtensions
{
    public static float AddPercentage(this float number, float percentage)
    {
        return number + (number * (percentage / 100));
    }
}
