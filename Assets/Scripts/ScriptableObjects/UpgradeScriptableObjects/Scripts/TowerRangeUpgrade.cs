using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerRangeUpgrade", menuName = "ScriptableObjects/Upgrades/TowerRangeUpgrade")]
public class TowerRangeUpgrade : Upgrade<RangeModule>
{
    public float Percentage;

    protected override Action<RangeModule> ComponentAction => (component) =>
            {
                component.RangeValue.BaseValue = component.RangeValue.BaseValue.AddPercentage(Percentage);
            };
}