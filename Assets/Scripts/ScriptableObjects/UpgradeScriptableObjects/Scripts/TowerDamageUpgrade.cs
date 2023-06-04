using System;
using UnityEngine;

public class TowerDamageUpgrade : Upgrade<DamageComponent>
{
    [SerializeField] private float m_increasePercentage;

    public override Action<DamageComponent> ComponentAction
    {
        get
        {
            return (component) =>
            {
                component.Damage = component.Damage.AddPercentage(20);
            };
        }
    }
}