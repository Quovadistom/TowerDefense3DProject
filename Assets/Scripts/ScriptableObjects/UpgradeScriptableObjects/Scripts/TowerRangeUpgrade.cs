using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerRangeUpgrade", menuName = "ScriptableObjects/Upgrades/TowerRangeUpgrade")]
public class TowerRangeUpgrade : Upgrade<RangeComponent>
{
    public float Percentage;

    protected override Action<RangeComponent> ComponentAction => (component) =>
            {
                component.BaseRange = component.BaseRange.AddPercentage(Percentage);
            };
}