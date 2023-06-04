using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthBoost", menuName = "ScriptableObjects/Boosts/GameBoosts/HealthBoost")]
public class HealthUpgrade : Upgrade<HealthComponent>
{
    [SerializeField] private int m_healthBoost;

    public override Action<HealthComponent> ComponentAction => (component) =>
    {
        component.Health += m_healthBoost;
    };
}