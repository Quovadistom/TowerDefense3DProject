using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerRangeModification", menuName = "ScriptableObjects/Modifications/TowerRangeModification")]
public class TowerRangeModification : Modification<RangeModule>
{
    public float Percentage;

    protected override Action<RangeModule> ComponentAction => (component) =>
            {
                component.RangeValue.BaseValue = component.RangeValue.BaseValue.AddPercentage(Percentage);
            };
}