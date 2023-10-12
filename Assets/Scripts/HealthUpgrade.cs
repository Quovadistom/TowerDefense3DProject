using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthEnhancement", menuName = "ScriptableObjects/Enhancements/GameEnhancements/HealthEnhancement")]
public class HealthUpgrade : TownUpgrade<HealthComponent>
{
    public int HealthEnhancement;

    protected override Action<HealthComponent> ComponentAction => (component) =>
    {
        component.Health += HealthEnhancement;
    };
}
