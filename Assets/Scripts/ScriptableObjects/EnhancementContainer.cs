using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class ModificationContainer
{
    [SerializeField] private string m_name;
    [SerializeField] private SerializableGuid m_id;
    [SerializeField] private ModificationType m_modificationType;

    public Guid TargetObjectID { get; set; } = Guid.Empty;
    public ModificationType ModificationType => m_modificationType;

    public ModuleModificationBase[] Modifications;
    public string Name => m_name;
    public Guid ID => m_id;
    public Rarity Rarity = Rarity.Common;
    public GameObject Visual;

    public bool IsModificationSuitable(ModuleParent towerInfoComponent) => Modifications.All(x => x.IsObjectSuitable(towerInfoComponent));

    public void ApplyModifications(ModuleParent towerInfoComponent)
    {
        foreach (ModuleModificationBase modification in Modifications)
        {
            modification.TryApplyModification(towerInfoComponent);
        }
    }
}
