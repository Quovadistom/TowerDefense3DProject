using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ComponentParent : MonoBehaviour
{
    private List<ComponentWithUpgradeBase> m_upgradableComponents = new();
    private BoostService m_boostService;
    private bool m_isInitialized = false;

    public List<ComponentWithUpgradeBase> UpgradableComponents
    {
        get
        {
            if (!m_isInitialized || gameObject.scene == default)
            {
                m_upgradableComponents = gameObject.GetComponentsInChildren<ComponentWithUpgradeBase>(true).ToList();
                m_isInitialized = true;
            }

            return m_upgradableComponents;
        }
    }

    public SerializableGuid ComponentID = (SerializableGuid)Guid.Empty;

    [Inject]
    private void Construct(BoostService boostService)
    {
        m_boostService = boostService;
    }

    protected virtual void Awake()
    {
        m_boostService.ApplyUpgradesToObject(this);
        m_boostService.UpgradeReceived += OnUpgradeReceived;
    }

    protected virtual void OnDestroy()
    {
        m_boostService.UpgradeReceived -= OnUpgradeReceived;
    }

    private void OnUpgradeReceived(UpgradeBase upgrade)
    {
        if (upgrade.IsObjectSuitable(this))
        {
            upgrade.TryApplyUpgrade(this);
        }
    }

    public bool HasComponent<T>() => UpgradableComponents.FirstOrDefault(component => component.HasComponent<T>()) != null;

    public bool TryFindAndActOnComponent<T>(Action<T> func)
    {
        bool upgradeSucces = false;
        foreach (ComponentWithUpgradeBase upgradable in UpgradableComponents)
        {
            if (!upgradable.TryFindAndActOnComponent(func))
            {
                continue;
            }

            upgradeSucces = true;
        }

        if (!upgradeSucces)
        {
            Debug.LogWarning($"Upgrade failed, there is no component of {typeof(T)} on {gameObject.name}", this);
        }

        return upgradeSucces;
    }
}
