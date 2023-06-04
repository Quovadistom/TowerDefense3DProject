using UnityEngine;

public abstract class UpgradeBase : ScriptableObject
{
    public abstract bool IsObjectSuitable(ComponentParent componentParent);

    public abstract void TryApplyUpgrade(ComponentParent componentParent);
}