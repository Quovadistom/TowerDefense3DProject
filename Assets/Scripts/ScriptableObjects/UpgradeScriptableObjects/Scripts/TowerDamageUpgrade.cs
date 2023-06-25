using System;
using UnityEngine;

public class TowerDamageUpgrade : Upgrade<DamageComponent>
{
    [SerializeField] private float m_increasePercentage;

    protected override Action<DamageComponent> ComponentAction => (component) =>
            {
                component.Damage.Value = component.Damage.Value.AddPercentage(20);
            };
}