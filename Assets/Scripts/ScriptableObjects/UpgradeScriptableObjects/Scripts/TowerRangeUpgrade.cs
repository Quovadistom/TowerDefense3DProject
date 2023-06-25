using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerRangeUpgrade", menuName = "ScriptableObjects/Upgrades/TowerRangeUpgrade")]
public class TowerRangeUpgrade : Upgrade<RangeComponent>
{
    public float Percentage;

    protected override Action<RangeComponent> ComponentAction => (component) =>
            {
                component.RangeValue.BaseValue = component.RangeValue.BaseValue.AddPercentage(Percentage);
            };
}