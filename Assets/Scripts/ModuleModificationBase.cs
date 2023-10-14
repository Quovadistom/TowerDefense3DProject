using UnityEngine;

public abstract class ModuleModificationBase : ScriptableObject
{
    public abstract bool IsObjectSuitable(ModuleParent componentParent);

    public abstract void TryApplyModification(ModuleParent componentParent);
}