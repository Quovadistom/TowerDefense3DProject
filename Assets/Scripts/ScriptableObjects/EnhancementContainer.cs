using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnhancementContainer
{
    [SerializeField] private string m_name;
    [SerializeField] private SerializableGuid m_id;
    [SerializeField] private EnhancementType m_enhancementType;

    public Guid TargetObjectID { get; set; } = Guid.Empty;
    public EnhancementType EnhancementType => m_enhancementType;

    public ModuleModificationBase[] Upgrades;
    public string Name => m_name;
    public Guid ID => m_id;
    public Rarity Rarity = Rarity.Common;
    public GameObject Visual;

    public bool IsEnhancementSuitable(ModuleParent towerInfoComponent) => Upgrades.All(x => x.IsObjectSuitable(towerInfoComponent));

    public void ApplyUpgrades(ModuleParent towerInfoComponent)
    {
        foreach (ModuleModificationBase upgrade in Upgrades)
        {
            upgrade.TryApplyUpgrade(towerInfoComponent);
        }
    }
}
