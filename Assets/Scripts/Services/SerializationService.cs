using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializationService
{
    public event Action SerializationRequested;

    public void RequestSerialization()
    {
        SerializationRequested?.Invoke();
    }
}
