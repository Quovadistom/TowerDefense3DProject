using NaughtyAttributes;
using System;
using UnityEngine;

public class UpgradeBase : ScriptableObject
{
    [ReadOnly][SerializeField] private string m_id;

    [SerializeField] protected string m_upgradeName;
    [InfoBox("Select the new visual (optional)")]
    [SerializeField] protected Transform m_newVisual;

    public string ID => m_id.ToString();

    public string Name => m_upgradeName;

    private void Reset()
    {
        m_id = Guid.NewGuid().ToString();
    }
}