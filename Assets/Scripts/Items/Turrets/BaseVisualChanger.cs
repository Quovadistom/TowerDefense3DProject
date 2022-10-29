using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseVisualChanger : MonoBehaviour
{
    [SerializeField] TurretMediatorBase m_turretMediatorBase;
    [SerializeField] GameObject m_baseVisualToChange;

    private void Awake()
    {
        m_turretMediatorBase.RangeVisualUpdated += OnRangeVisualUpdated;
    }

    private void OnDestroy()
    {        
        m_turretMediatorBase.RangeVisualUpdated += OnRangeVisualUpdated;
    }

    private void OnRangeVisualUpdated(GameObject newVisual)
    {
        GameObject visual = GameObject.Instantiate(newVisual, m_baseVisualToChange.transform.parent);
        Destroy(m_baseVisualToChange);
        m_baseVisualToChange = visual;
    }
}
