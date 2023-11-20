using UnityEngine;

public static class FloatExtensions
{
    public static float AddPercentage(this float number, float percentage)
    {
        return number + (number * (percentage / 100));
    }

    public static float PercentageOf(this float number, float percentage)
    {
        return (number * (percentage / 100));
    }

    public static float RemovePercentage(this float number, float percentage)
    {
        return Mathf.Clamp(number - (number * (percentage / 100)), 0, number);
    }

    public static int AddPercentage(this int number, float percentage)
    {
        return Mathf.FloorToInt(number + (number * (percentage / 100)));
    }

    public static int RemovePercentage(this int number, float percentage)
    {
        return Mathf.FloorToInt(Mathf.Clamp(number - (number * (percentage / 100)), 0, number));
    }
}
