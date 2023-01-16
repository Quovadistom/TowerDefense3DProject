using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenuService
{
    public event Action<List<ButtonInfo>> ItemMenuRequested;
    public event Action ItemMenuCloseRequested;

    public void RequestItemMenu(List<ButtonInfo> buttonInfos)
    {
        ItemMenuRequested?.Invoke(buttonInfos);
    }

    public void RequestCloseItemMenu()
    {
        ItemMenuCloseRequested?.Invoke();
    }
}
