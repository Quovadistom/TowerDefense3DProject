using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ModuleParent : MonoBehaviour
{
    private List<ModuleWithModificationBase> m_upgradableComponents = new();
    private ModuleModificationService m_enhancementService;
    private bool m_isInitialized = false;

    public List<ModuleWithModificationBase> UpgradableComponents
    {
        get
        {
            if (!m_isInitialized || gameObject.scene == default)
            {
                m_upgradableComponents = gameObject.GetComponentsInChildren<ModuleWithModificationBase>(true).ToList();
                m_isInitialized = true;
            }

            return m_upgradableComponents;
        }
    }

    public Guid ID { get; set; }

    [Inject]
    private void Construct(ModuleModificationService enhancementService)
    {
        m_enhancementService = enhancementService;
    }

    protected virtual void Awake()
    {
        m_enhancementService.ApplyEnhancementsToObject(this);
        m_enhancementService.UpgradeReceived += OnUpgradeReceived;
    }

    protected virtual void OnDestroy()
    {
        m_enhancementService.UpgradeReceived -= OnUpgradeReceived;
    }

    private void OnUpgradeReceived(ModuleModificationBase upgrade)
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
        foreach (ModuleWithModificationBase upgradable in UpgradableComponents)
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
