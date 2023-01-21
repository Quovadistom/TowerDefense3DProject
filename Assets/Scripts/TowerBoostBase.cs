using System;
using UnityEngine;

public abstract class TowerBoostBase : BoostBase
{
    public abstract Type TowerComponentType { get; }

    public abstract void ApplyBoost(TowerInfoComponent towerInfoComponent);
}
