using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static bool TryRemove<T>(this List<T> list, T value)
    {
        if (list.Contains(value))
        {
            list.Remove(value);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Adds a value to a list if the value is not yet present
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="value"></param>
    public static void AddSafely<T>(this List<T> list, T value)
    {
        if (list.Contains(value))
        {
            return;
        }

        list.Add(value);
    }
}
