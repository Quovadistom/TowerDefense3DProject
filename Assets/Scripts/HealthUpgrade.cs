using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthModification", menuName = "ScriptableObjects/Modifications/GameModifications/HealthModification")]
public class HealthModification : TownModification<HealthModule>
{
    public int HealthValue;

    protected override Action<HealthModule> ComponentAction => (component) =>
    {
        component.Health += HealthValue;
    };
}
