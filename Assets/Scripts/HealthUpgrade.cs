using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthBoost", menuName = "ScriptableObjects/Boosts/GameBoosts/HealthBoost")]
public class HealthUpgrade : Upgrade<HealthComponent>
{
    public int HealthBoost;

    protected override Action<HealthComponent> ComponentAction => (component) =>
    {
        component.Health += HealthBoost;
    };
}