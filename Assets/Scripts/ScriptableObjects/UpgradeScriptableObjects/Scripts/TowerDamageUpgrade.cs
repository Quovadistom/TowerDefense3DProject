using System;
using UnityEngine;

public class TowerDamageModification : Modification<DamageModule>
{
    [SerializeField] private float m_increasePercentage;

    protected override Action<DamageModule> ComponentAction => (component) =>
            {
                component.Damage.Value = component.Damage.Value.AddPercentage(20);
            };
}