using System;
using UnityEngine;

public abstract class TowerBoostBase : MonoBehaviour
{
    public string UpgradeName;
    public Sprite UpgradeVisual;

    public string UpgradeID => UpgradeName.Replace(" ", "");

    public abstract Type TowerComponentType { get; }

    public abstract void ApplyBoost(TowerInfoComponent towerInfoComponent);
}
