using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthEnhancement", menuName = "ScriptableObjects/Enhancements/GameEnhancements/HealthEnhancement")]
public class HealthUpgrade : TownUpgrade<HealthModule>
{
    public int HealthEnhancement;

    protected override Action<HealthModule> ComponentAction => (component) =>
    {
        component.Health += HealthEnhancement;
    };
}
