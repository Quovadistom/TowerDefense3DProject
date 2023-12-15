using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ModuleParent : MonoBehaviour
{
    private List<ModuleWithModificationBase> m_upgradableModules = new();
    private ModuleModificationService m_modificationService;
    private bool m_isInitialized = false;

    public List<ModuleWithModificationBase> UpgradableModules
    {
        get
        {
            if (!m_isInitialized || gameObject.scene == default)
            {
                m_upgradableModules = gameObject.GetComponentsInChildren<ModuleWithModificationBase>(true).ToList();
                m_isInitialized = true;
            }

            return m_upgradableModules;
        }
    }

    public Guid ID { get; set; }

    [Inject]
    private void Construct(ModuleModificationService modificationService)
    {
        m_modificationService = modificationService;
    }

    protected virtual void Awake()
    {
        m_modificationService.ApplyBlueprintsToObject(this);
        m_modificationService.ModificationReceived += OnModificationReceived;
    }

    protected virtual void OnDestroy()
    {
        m_modificationService.ModificationReceived -= OnModificationReceived;
    }

    private void OnModificationReceived(ModuleModificationBase modification)
    {
        if (modification.IsObjectSuitable(this))
        {
            modification.TryApplyModification(this);
        }
    }

    public bool HasModule<T>() => UpgradableModules.FirstOrDefault(component => component.HasModule<T>()) != null;

    public bool TryFindAndActOnModule<T>(Action<T> func)
    {
        bool modificationSucces = false;
        foreach (ModuleWithModificationBase upgradable in UpgradableModules)
        {
            if (!upgradable.TryFindAndActOnModule(func))
            {
                continue;
            }

            modificationSucces = true;
        }

        if (!modificationSucces)
        {
            Debug.LogWarning($"Modification failed, there is no component of {typeof(T)} on {gameObject.name}", this);
        }

        return modificationSucces;
    }
}
