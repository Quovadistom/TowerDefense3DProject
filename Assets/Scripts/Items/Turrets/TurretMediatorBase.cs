using System;
using UnityEngine;

public class TurretMediatorBase : MonoBehaviour
{
    [Header("Range UI References")]
    public TurretUpgradeTreeBase UpgradeTreeAsset;

    [Header("Range Settings")]
    [SerializeField] private float m_range = 4;
    
    private GameObject m_rangeVisual;

    public event Action<float> RangeUpdated;
    public event Action<GameObject> RangeVisualUpdated;

    protected virtual void Start()
    {
        Range = m_range;
    }

    public float Range
    {
        get => m_range;
        set
        {
            m_range = value;
            RangeUpdated?.Invoke(m_range);
        }
    }

    public GameObject RangeVisual
    {
        get => m_rangeVisual;
        set
        {
            m_rangeVisual = value;
            RangeVisualUpdated?.Invoke(m_rangeVisual);
        }
    }
}