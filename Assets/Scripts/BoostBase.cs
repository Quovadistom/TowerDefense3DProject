using UnityEngine;

public abstract class BoostBase : ScriptableObject
{
    public string UpgradeName;
    public Sprite UpgradeVisual;

    public string UpgradeID => UpgradeName.Replace(" ", "");
}
