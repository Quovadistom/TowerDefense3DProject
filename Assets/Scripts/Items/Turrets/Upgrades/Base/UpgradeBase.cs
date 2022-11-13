using System;
using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(UpgradeNode))]
public class UpgradeBase<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private int m_upgradeCost;
    public T m_turretMediator;

    private UpgradeNode m_node;
    private LevelService m_levelService;

    [Inject]
    public void Construct(LevelService levelService)
    {
        m_levelService = levelService;
    }

    private void Awake()
    {
        m_node = GetComponent<UpgradeNode>();
        m_node.ButtonClicked += OnUpgradeButtonClicked;
    }

    private void Reset()
    {
#if UNITY_EDITOR
        if (m_turretMediator == null)
        {
            m_turretMediator = GetComponentInParent<T>();

            if (m_turretMediator == null)
            {
                Debug.LogError($"Turret mediator {typeof(T)} not found! Destroying this object...", this);
                DestroyImmediate(gameObject);
            }
        }
#endif
    }

    protected virtual void OnUpgradeButtonClicked() 
    {
        m_levelService.Money -= m_upgradeCost;
    }
}