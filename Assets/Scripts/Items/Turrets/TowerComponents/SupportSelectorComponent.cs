using System;
using UnityEngine;

[Serializable]
public class SupportSelectorComponent : ComponentBase
{
    [SerializeField] private int m_allowedTowerAmount = 4;

    public event Action<int> AllowedTowerAmountChanged;

    public int AllowedTowerAmount
    {
        get => m_allowedTowerAmount;
        set
        {
            m_allowedTowerAmount = value;
            AllowedTowerAmountChanged?.Invoke(value);
        }
    }
}
